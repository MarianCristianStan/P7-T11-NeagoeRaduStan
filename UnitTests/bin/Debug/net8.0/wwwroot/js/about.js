const carouselItems = document.querySelectorAll('.carousel-item');
let currentIndex = 0;

function showNextItem() {
   carouselItems[currentIndex].classList.remove('active');
   currentIndex = (currentIndex + 1) % carouselItems.length;
   carouselItems[currentIndex].classList.add('active');
}

// Verifică dacă este primul element
if (currentIndex === 0) {
   carouselItems[0].classList.add('active');
}

setInterval(showNextItem, 7000); // Schimbă elementul la fiecare 7 secunde
