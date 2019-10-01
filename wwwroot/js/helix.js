// SCSS Color options
/*
$c_c : rgb(255, 187, 141) rgb(255, 115, 115) rgb(250, 251, 252); // top / bottom / BG 
$c_bII : rgba(0,227,255,1) rgba(0,227,255,0.5) rgb(89,98,255); // top / bottom / BG 
$c_b : rgb(2, 255, 227) rgba(2, 255, 255,0.667) rgb(102,40,255); // top / bottom / BG 
*/

$(document).ready(function () {

    // RegEx psuedo SpitText
    $('.h1wrap').each(function () {
        $(this).find("h1").html($(this).find("h1").html().replace(/./g, "<span>$&</span>").replace(/\s/g, "&nbsp;"));
    });


    function introOpen() {
        //Using ex. transform:"translate3d(x,x,x)" instead of (y:__) or (top:__) to maintain vh or em units responsiveness
        TweenMax.staggerFrom("h1 > span", 1.2, { opacity: 0, transform: "translateY(16vh) scaleY(-0.382)", transformOrigin: '50% 20%', ease: Expo.easeOut, force3D: true }, 0.1)

        TweenMax.from("h1", 3.6, { transform: "translateY(16vh)", ease: Expo.easeOut, force3D: true })

        TweenMax.staggerFrom(".nucleobase", 1.2, { opacity: 0.0, transform: "translateY(20vh) scale(0)", delay: 0.333, transformOrigin: '50% 50%', ease: Circ.easeOut, force3D: true }, 0.06)
    };

    introOpen();

});