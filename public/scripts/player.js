document.addEventListener('DOMContentLoaded', () => {
    const uuid = localStorage.getItem('uuid') || crypto.randomUUID();
    const socket = io({ query: { uuid } });
    localStorage.setItem('uuid', uuid);
    const sphere = document.querySelector('#sphere');
    const status = document.querySelector('#status');

    socket.on('updateBoxHp', (newHp) => {
        console.log('updateBoxHp received:', newHp);
        const currentBox = document.querySelector('#movingBox');
        if (!currentBox) return;
        // Only update visibility here, not color
        currentBox.setAttribute('visible', newHp > 0);
    });

    socket.on('connect', () => {
        status.textContent = '✅ 接続成功！';
    });

    socket.on('disconnect', () => {
        status.textContent = '❌ 接続が切れました';
    });

    socket.on('sceneUpdate', (sceneName) => {
        console.log('🔁 シーン更新:', sceneName);
        let textureId = sceneName === 'space' ? '#space' : '#earth';
        sphere.setAttribute('material', 'src', textureId);
    });

    socket.on('boxPosition', (pos) => {
        const currentBox = document.querySelector('#movingBox');
        if (currentBox) currentBox.setAttribute('position', pos);
    });

    socket.on('boxColor', (color) => {
        const currentBox = document.querySelector('#movingBox');
        if (currentBox) currentBox.setAttribute('color', color);
    });

    socket.on('spawnBox', (boxData) => {
        const oldBox = document.querySelector(`#${boxData.id}`);
        if (oldBox) oldBox.parentNode.removeChild(oldBox);

        const scene = document.querySelector('a-scene');
        const box = document.createElement('a-box');
        box.setAttribute('id', boxData.id);
        box.setAttribute('class', 'clickable');
        box.setAttribute('position', boxData.position);
        box.setAttribute('color', boxData.color);
        box.setAttribute('depth', 1);
        box.setAttribute('height', 1);
        box.setAttribute('width', 1);
        box.setAttribute('rotation', '0 45 0');
        box.setAttribute('damage-on-hover', '');
        scene.appendChild(box);
    });

    // Move this inside the DOMContentLoaded callback
    AFRAME.registerComponent('damage-on-hover', {
        init: function () {
            let intervalId = null;
            this.el.addEventListener('mouseenter', () => {
                if (intervalId) return;
                socket.emit('startDamage');
                intervalId = setInterval(() => {
                    socket.emit('hitBox');
                }, 200);
            });
            this.el.addEventListener('mouseleave', () => {
                if (intervalId) {
                    clearInterval(intervalId);
                    intervalId = null;
                }
                socket.emit('stopDamage');
            });
        }
    });
});