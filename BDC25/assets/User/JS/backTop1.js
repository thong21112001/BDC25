var btntop = document.querySelector('.img-on-bot');
////thanh menu doi mau
window.addEventListener('scroll', function () {
    if (this.window.pageYOffset > 50) {
        btntop.classList.add('img-on-bots');
    }
    else {
        btntop.classList.remove('img-on-bots');
    }
})
// click trở về đầu trang
btntop.onclick = function BackTop() {
    document.documentElement.scrollTop = 0;
}