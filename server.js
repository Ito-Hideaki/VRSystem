const express = require('express');
const http = require('http');
const socketIo = require('socket.io');
const os = require('os');

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

const PORT = 1729;
const players = {}; // uuid => { socketId, connectedAt, points }
let boxHp = 100;
let boxPos = { x: 0, y: 1, z: -3 };
let direction = 1;

app.use(express.static('public'));

io.on('connection', (socket) => {
    const isAdmin = socket.handshake.query.admin === 'true';
    const uuid = socket.handshake.query.uuid;

    function boxDestroyed() {
        console.log("boxが破壊された！");
        if (uuid && players[uuid]) {
            players[uuid].points = (players[uuid].points || 0) + 1;
            io.emit('updatePoints', getAllPlayerPoints());
        }
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
            points: players[uuid]?.points || 0
        };

        // Send current box state and points to the new client only
        socket.emit('spawnBox', {
            id: 'movingBox',
            position: boxPos,
            color: '#4CC3D9'
        });
        socket.emit('updateBoxHp', boxHp);
        socket.emit('updatePoints', getAllPlayerPoints());

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
                io.emit('updatePoints', getAllPlayerPoints());
            }
        } else {
            console.log(`👑 管理者切断: ${socket.id}`);
        }
    });
});

// Helper to get all player points
function getAllPlayerPoints() {
    // { uuid: points, ... }
    const result = {};
    for (const uuid in players) {
        result[uuid] = players[uuid].points || 0;
    }
    return result;
}

// Box movement direction vector
let moveDir = randomDirection();

function randomDirection() {
    // Generate a random normalized direction vector (x, y, z)
    let dx = (Math.random() - 0.5);
    let dy = (Math.random() - 0.5) * 0.5; // less vertical movement
    let dz = (Math.random() - 0.5);
    // Normalize
    const len = Math.sqrt(dx*dx + dy*dy + dz*dz);
    return {
        x: dx / len,
        y: dy / len,
        z: dz / len
    };
}

// Move the box smoothly in a direction until hitting a boundary, then pick a new direction
setInterval(() => {
    // Define movement boundaries
    const minX = -2, maxX = 2;
    const minY = 1, maxY = 2;
    const minZ = -4, maxZ = -2;

    // Movement speed
    const speed = 0.07;

    // Move in current direction
    boxPos.x += moveDir.x * speed;
    boxPos.y += moveDir.y * speed;
    boxPos.z += moveDir.z * speed;

    // Check boundaries and pick new direction if needed
    let bounced = false;
    if (boxPos.x < minX) { boxPos.x = minX; bounced = true; }
    if (boxPos.x > maxX) { boxPos.x = maxX; bounced = true; }
    if (boxPos.y < minY) { boxPos.y = minY; bounced = true; }
    if (boxPos.y > maxY) { boxPos.y = maxY; bounced = true; }
    if (boxPos.z < minZ) { boxPos.z = minZ; bounced = true; }
    if (boxPos.z > maxZ) { boxPos.z = maxZ; bounced = true; }

    // If bounced, pick a new random direction
    if (bounced) {
        moveDir = randomDirection();
    }

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
