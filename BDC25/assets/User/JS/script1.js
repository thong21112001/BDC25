///slide 1
let slideIndex = 1;
showSlides(slideIndex);

function plusSlides(n) {
    showSlides(slideIndex += n);
}

function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    let i;
    let slides = document.getElementsByClassName("images-slide-background");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    slides[slideIndex - 1].style.display = "block";

}
setInterval(function () {
    plusSlides(1);
}, 3000);

// slide 2

var slidesIndex = 1;
showDivs(slidesIndex);

function plusDivs(n) {
    showDivs(slidesIndex += n);
}

function currentDiv(n) {
    showDivs(slidesIndex = n);
}

function showDivs(n) {
    var i;
    var x = document.getElementsByClassName("title-slide-six");
    var dots = document.getElementsByClassName("demo");
    if (n > x.length) { slidesIndex = 1 }
    if (n < 1) { slidesIndex = x.length }
    for (i = 0; i < x.length; i++) {
        x[i].style.visibility = "hidden";
    }


    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active-gray", "");
    }
    x[slidesIndex - 1].style.visibility = "inherit";
    dots[slidesIndex - 1].className += " active-gray";
}

// slide 3
var slideIndex3 = 1;
showSlides3(slideIndex3);

var slideInterval = setInterval(function () {
    plusSlides3(1);
}, 3000); // change the time interval (in milliseconds) as needed

function plusSlides3(n) {
    clearInterval(slideInterval);
    slideInterval = setInterval(function () {
        plusSlides3(1);
    }, 3000); // reset the interval timer
    showSlides3(slideIndex3 += n);
}

function currentSlides(n) {
    clearInterval(slideInterval);
    slideInterval = setInterval(function () {
        plusSlides3(1);
    }, 3000); // reset the interval timer
    showSlides3(slideIndex3 = n);
}

function showSlides3(n) {
    var i;
    var slides3 = document.getElementsByClassName("content-slide-seven");
    var dots = document.getElementsByClassName("dots-seven");
    if (n > Math.ceil(slides3.length / 3)) {
        slideIndex3 = 1;
    }
    if (n < 1) {
        slideIndex3 = Math.ceil(slides3.length / 2);
    }
    for (i = 0; i < slides3.length; i++) {
        slides3[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    var start = (slideIndex3 - 1) * 3;
    var end = start + 3;
    for (i = start; i < end; i++) {
        if (slides3[i]) {
            slides3[i].style.display = "block";
        }
    }
    dots[slideIndex3 - 1].className += " active";
}

//auto-slide
var myIndex = 0;
carousel();

function carousel() {
    var i;
    var x = document.getElementsByClassName("auto-slide");
    for (i = 0; i < x.length; i++) {
        x[i].style.display = "none";
    }
    myIndex++;
    if (myIndex > x.length) { myIndex = 1 }
    x[myIndex - 1].style.display = "block";
    setTimeout(carousel, 2000); // Change image every 2 seconds
}