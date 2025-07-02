const socket = io({ query: { admin: 'true' } });

function changeScene(scene) {
    console.log('🔘 シーン切り替え:', scene);
    socket.emit('sceneChange', scene);
}

let playerPoints = {};

socket.on('updatePoints', pointsObj => {
    playerPoints = pointsObj;
    updatePlayerList();
});

socket.on('updatePlayerList', (playerUuids) => {
    updatePlayerList(playerUuids);
});

function updatePlayerList(playerUuids) {
    const list = document.getElementById('player-list');
    list.innerHTML = '';
    (playerUuids || Object.keys(playerPoints)).forEach(uuid => {
        const li = document.createElement('li');
        const points = playerPoints[uuid] !== undefined ? playerPoints[uuid] : 0;
        li.textContent = `${uuid} : ${points}pt`;
        list.appendChild(li);
    });
}