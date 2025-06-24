const socket = io({ query: { admin: 'true' } });

function changeScene(scene) {
    console.log('🔘 シーン切り替え:', scene);
    socket.emit('sceneChange', scene);
}

socket.on('updatePlayerList', (playerUuids) => {
    const list = document.getElementById('player-list');
    list.innerHTML = '';
    playerUuids.forEach(uuid => {
        const li = document.createElement('li');
        li.textContent = uuid;
        list.appendChild(li);
    });
});