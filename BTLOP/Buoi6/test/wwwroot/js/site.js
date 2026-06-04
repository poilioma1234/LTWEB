(() => {
  const hero = document.querySelector(".shop-hero");

  if (hero && window.Swiper) {
    new Swiper(hero, {
      autoplay: {
        delay: 3500,
        disableOnInteraction: false
      },
      loop: true,
      pagination: {
        el: ".swiper-pagination",
        clickable: true
      }
    });
  }
})();
