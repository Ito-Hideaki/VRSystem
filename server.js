const express = require('express');
const http = require('http');
const socketIo = require('socket.io');
const os = require('os');

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

const PORT = 1729;
const players = {};

app.use(express.static('public'));

io.on('connection', (socket) => {
    const isAdmin = socket.handshake.query.admin === 'true';

    if (!isAdmin) {
        players[socket.id] = true;
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
            delete players[socket.id];
            io.emit('updatePlayerList', Object.keys(players));
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
