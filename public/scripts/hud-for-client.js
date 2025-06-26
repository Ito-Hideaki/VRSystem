var HUDFC /* HUD For Client */ = (() => {
    return {
        createHUDElement: function() {
            return document.createElement("div");
        },

        /** pointHUDのstyleを考える
         * @return style */
        thinkPointHUDStyle: function(point, maxPoint) {
            const isPointMax = point >= maxPoint;
            const gaugeLength = (isPointMax ? 1 : point / maxPoint) * 48;
            const style = {
                width: `${gaugeLength}vw`,
                height: "5vw",
                border: "solid black",
                borderWidth: `1vw ${49 - gaugeLength}vw 1vw 1vw`,

                color: "white",
                backgroundColor: isPointMax ? "green" : "blue",
                fontFamily: "monospace",
                fontSize: "4vw",
                textAlign: "left",
                overflow: "visible",
                textWrap: "nowrap",
                opacity: "0.7",
            };
            return style;
        },

        /** @return { Number } maxPoint */
        thinkPointHUDMaxPoint: function() {
            return 20;
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
            elm.textContent = content;
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