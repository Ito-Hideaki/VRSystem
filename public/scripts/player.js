document.addEventListener('DOMContentLoaded', () => {
    const uuid = localStorage.getItem('uuid') || crypto.randomUUID();
    const socket = io({ query: { uuid } });
    localStorage.setItem('uuid', uuid);
    const sphere = document.querySelector('#sphere');
    const status = document.querySelector('#status');
    const box = document.querySelector('#movingBox');

    socket.on('updateBoxHp', (newHp) => {
        console.log(`❤️ boxの残HP: ${newHp}`);
        box.hp = newHp;

        // 色を変更（HPによって変化）
        const red = Math.min(255, (100 - newHp) * 2.5);
        const color = `rgb(${red}, 100, 100)`;
        box.setAttribute('color', color);

        // HPが0になったら消す
        if (newHp <= 0) {
            box.setAttribute('visible', false);
        }
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

    let direction = 1;
    const pos = box.getAttribute('position');
    setInterval(() => {
        if (pos.x > 2 || pos.x < -2) direction *= -1;
        pos.x += 0.05 * direction;
        box.setAttribute('position', pos);
    }, 50);
});

// A-Frame component registration should remain outside DOMContentLoaded
AFRAME.registerComponent('damage-on-hover', {
    schema: {
        health: { type: 'int', default: 100 }
    },
    init: function () {
        let intervalId = null;
        const DAMAGE = 10;
        const INTERVAL = 200; // ms

        this.el.addEventListener('mouseenter', evt => {
            if (intervalId) return; // Already damaging
            intervalId = setInterval(() => {
                if (this.data.health > 0) {
                    this.data.health -= DAMAGE;
                    this.el.setAttribute('color', '#F00');
                    setTimeout(() => {
                        this.el.setAttribute('color', '#4CC3D9');
                    }, 100);
                    if (typeof socket !== 'undefined') {
                        socket.emit('hitBox', this.data.health);
                    }
                    if (this.data.health <= 0) {
                        this.el.setAttribute('visible', false);
                        clearInterval(intervalId);
                        intervalId = null;
                    }
                }
            }, INTERVAL);
        });

        this.el.addEventListener('mouseleave', evt => {
            if (intervalId) {
                clearInterval(intervalId);
                intervalId = null;
            }
        });
    }
});