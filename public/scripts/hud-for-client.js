var HUDFC /* HUD For Client */ = (() => {
    return {
        createHUDElement: function() {
            return document.createElement("a-text");
        },

        /** @return { Number } maxPoint */
        thinkPointHUDMaxPoint: function() {
            return 20;
        },

        createPointTextHUD: function() {
            const hud = document.createElement("a-text");
            hud.setAttribute("color", "black");
            hud.setAttribute("font", "dejavu");
            hud.setAttribute("scale", "0.006 0.006 0.01");
            return hud;
        },

        thinkPointHUDResponsivePosition: function() {
            return {
                x: -7 * 0.001 * document.getElementsByTagName("a-scene")[0].clientWidth / (10 + document.getElementsByTagName("a-scene")[0].clientHeight),
                y: 13 * 0.0005,
                z: -0.01
            };
        },

        /** pointHUDのテキストを考える
         * @param { number } point
         * @return { string } content */
        thinkPointHUDContent: function( point ) {
            return `${point} Point`;
        },

        /** HUDにstyleを適用する
         * @param elm
         * @param style */
        setStyle: function( elm, style ) {
            Object.assign(elm.style, style);
        },

        /** HUDの表示を設定する
        * @param { HTMLElement } elm
        * @param { String } content */
        setHUDContent: function( elm, content ) {
            elm.setAttribute("value", content)
        },

        /** HUDの左上xy座標を設定する
        * @param { HTMLElement } elm
        * @param { String } distanceFromTop
        * @param { String } distanceFromLeft */
        setHUDFixedPosition: function( elm, distanceFromTop, distanceFromLeft ) {
            elm.style.position = "fixed";
            elm.style.top = distanceFromTop;
            elm.style.left = distanceFromLeft;
        },
    }
})();