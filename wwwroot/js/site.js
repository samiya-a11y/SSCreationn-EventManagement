// SS Creationn — front-end behaviour (UI only)
(function () {
    'use strict';

    // Scroll-reveal for any element marked .ssc-reveal
    var els = document.querySelectorAll('.ssc-reveal');
    if (!('IntersectionObserver' in window)) {
        els.forEach(function (e) { e.classList.add('ssc-in'); });
    } else {
        var io = new IntersectionObserver(function (entries) {
            entries.forEach(function (en) {
                if (en.isIntersecting) { en.target.classList.add('ssc-in'); io.unobserve(en.target); }
            });
        }, { threshold: 0.12 });
        els.forEach(function (e) { io.observe(e); });
    }

    // Navbar gets a soft shadow once the page is scrolled
    var nav = document.querySelector('.ssc-navbar');
    if (nav) {
        var onScroll = function () { nav.classList.toggle('is-scrolled', window.scrollY > 8); };
        onScroll();
        window.addEventListener('scroll', onScroll, { passive: true });
    }

    // Keyboard access for clickable cards that open a modal
    document.querySelectorAll('.ssc-card[data-bs-toggle="modal"]').forEach(function (c) {
        c.addEventListener('keydown', function (ev) {
            if (ev.key === 'Enter' || ev.key === ' ') { ev.preventDefault(); c.click(); }
        });
    });
})();
