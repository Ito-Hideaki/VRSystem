const express = require('express');
const http = require('http');
const socketIo = require('socket.io');
const os = require('os');

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

const PORT = 1729;

app.use(express.static('public'));

io.on('connection', (socket) => {
    console.log('🎉 クライアント接続:', socket.id);

    socket.on('sceneChange', (sceneName) => {
        console.log('📺 シーン変更要求:', sceneName);
        io.emit('sceneUpdate', sceneName);
    });

    socket.on('disconnect', () => {
        console.log('👋 切断:', socket.id);
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
