const express = require('express');
const http = require('http');
const socketIo = require('socket.io');
const os = require('os');

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

const PORT = 1729;
const players = {}; // uuid => { socketId, connectedAt }
let boxHp = 100;
let boxPos = { x: 0, y: 1, z: -3 };
let direction = 1;

app.use(express.static('public'));

io.on('connection', (socket) => {
    const isAdmin = socket.handshake.query.admin === 'true';
    const uuid = socket.handshake.query.uuid;

    function boxDestroyed() {
        console.log("boxが破壊された！");
    }

    if (!isAdmin) {
        if (!uuid) {
            console.log('❌ UUIDなし接続を拒否');
            socket.disconnect();
            return;
        }

        players[uuid] = {
            socketId: socket.id,
            connectedAt: new Date(),
        };

        // Send current box state to the new client only
        socket.emit('spawnBox', {
            id: 'movingBox',
            position: boxPos,
            color: '#4CC3D9'
        });
        socket.emit('updateBoxHp', boxHp);

        console.log(`🎮 プレイヤー接続: UUID=${uuid}, socketId=${socket.id}`);
        io.emit('updatePlayerList', Object.keys(players));
    } else {
        console.log(`👑 管理者接続: ${socket.id}`);
    }

    socket.on('sceneChange', (sceneName) => {
        console.log('📺 シーン変更要求:', sceneName);
        io.emit('sceneUpdate', sceneName);
    });

    socket.on('hitBox', () => {
        if (!isAdmin && boxHp > 0) {
            boxHp -= 10;
            console.log(`💥 box が攻撃された！残HP: ${boxHp}`);
            io.emit('updateBoxHp', boxHp);

            // If destroyed, respawn after a delay
            if (boxHp <= 0) {
                boxDestroyed();
                setTimeout(() => {
                    boxHp = 100;
                    boxPos = { x: 0, y: 1, z: -3 }; // or randomize if you want
                    io.emit('spawnBox', {
                        id: 'movingBox',
                        position: boxPos,
                        color: '#4CC3D9'
                    });
                    io.emit('updateBoxHp', boxHp);
                }, 2000); // 2 seconds respawn delay
            }
        }
    });

    socket.on('startDamage', () => {
        io.emit('boxColor', 'red');
    });

    socket.on('stopDamage', () => {
        io.emit('boxColor', '#4CC3D9');
    });

    socket.on('disconnect', () => {
        if (!isAdmin) {
            const uuidToDelete = Object.keys(players).find(key => players[key].socketId === socket.id);
            if (uuidToDelete) {
                delete players[uuidToDelete];
                console.log(`👋 プレイヤー切断: UUID=${uuidToDelete}`);
                io.emit('updatePlayerList', Object.keys(players));
            }
        } else {
            console.log(`👑 管理者切断: ${socket.id}`);
        }
    });
});

// Move the box on the server and broadcast position
setInterval(() => {
    if (boxPos.x > 2 || boxPos.x < -2) direction *= -1;
    boxPos.x += 0.05 * direction;
    io.emit('boxPosition', boxPos);
}, 50);

server.listen(PORT, () => {
    console.log(`🚀 サーバ起動 http://localhost:${PORT}`);
    const interfaces = os.networkInterfaces();
    Object.keys(interfaces).forEach((name) => {
        interfaces[name].forEach((iface) => {
            if (iface.family === 'IPv4' && !iface.internal) {
                console.log(`🌐 他端末からアクセス: http://${iface.address}:${PORT}`);
            }
        });
    });
});
