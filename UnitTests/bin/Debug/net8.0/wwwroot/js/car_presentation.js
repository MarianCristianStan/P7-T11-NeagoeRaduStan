document.addEventListener("DOMContentLoaded", function () {
  var counters = document.querySelectorAll(".counter");
  counters.forEach(function (counter) {
    var target = parseFloat(counter.getAttribute("data-target"));
    var speed = parseInt(counter.getAttribute("data-speed"));
    var count = 0;

    function updateCounter() {
      var increment = target / speed;

      if (count < target) {
        count += increment;
        counter.innerText = Math.floor(count);
        setTimeout(updateCounter, 1);
      } else {
        counter.innerText = target;
      }
    }

    updateCounter();
  });
});

document.addEventListener("DOMContentLoaded", function () {
   function isElementInViewport(el) {
      var rect = el.getBoundingClientRect();
      return (
         rect.top >= 0 &&
         rect.left >= 0 &&
         rect.bottom <=
         (window.innerHeight || document.documentElement.clientHeight) &&
         rect.right <= (window.innerWidth || document.documentElement.clientWidth)
      );
   }

   function handleScroll() {
      var specsSection = document.querySelector(".specs");

      if (isElementInViewport(specsSection)) {
         specsSection.classList.add("reveal");
      }
   }
   window.addEventListener("scroll", handleScroll);

   handleScroll();
});

function showNotification(message, event) {
  const notification = document.getElementById("notification");
  notification.innerText = message;

  const x = event.clientX;
  const y = event.clientY;
  notification.style.left = `${x}px`;
  notification.style.top = `${y}px`;

  notification.style.display = "block";

  setTimeout(() => {
    notification.style.display = "none";
  }, 5000);
}

document.addEventListener("DOMContentLoaded", function () {
  var priceCounter = document.querySelector(".price-counter");
  var priceTarget = parseFloat(priceCounter.getAttribute("data-target"));
  var priceSpeed = parseInt(priceCounter.getAttribute("data-speed"));
  var priceCount = 0;

  function updatePriceCounter() {
    var priceIncrement = priceTarget / priceSpeed;

    if (priceCount < priceTarget) {
      priceCount += priceIncrement;
      priceCounter.innerText = `$${Math.floor(priceCount).toLocaleString()}`;
      setTimeout(updatePriceCounter, 1);
    } else {
      priceCounter.innerText = `$${priceTarget.toLocaleString()}`;
      priceCounter.classList.add("reach-target");
    }
  }

  updatePriceCounter();
});
