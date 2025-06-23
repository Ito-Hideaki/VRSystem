const express = require('express');
const http = require('http');
const socketIo = require('socket.io');
const os = require('os');

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

const PORT = 1729;
const players = {}; // uuid => { socketId, connectedAt }

app.use(express.static('public'));

io.on('connection', (socket) => {
    const isAdmin = socket.handshake.query.admin === 'true';
    const uuid = socket.handshake.query.uuid;

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

        console.log(`🎮 プレイヤー接続: UUID=${uuid}, socketId=${socket.id}`);
        io.emit('updatePlayerList', Object.keys(players));
    } else {
        console.log(`👑 管理者接続: ${socket.id}`);
    }

    socket.on('sceneChange', (sceneName) => {
        console.log('📺 シーン変更要求:', sceneName);
        io.emit('sceneUpdate', sceneName);
    });

    socket.on('disconnect', () => {
        if (!isAdmin) {
            // UUIDを socket.id から逆引きして削除
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
