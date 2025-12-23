// SİLME ONAYI
function silOnayi() {
    return confirm("Bu kaydı silmek istediğinize emin misiniz?");
}

// KART HOVER EFEKTİ
document.addEventListener("DOMContentLoaded", function () {
    const cards = document.querySelectorAll(".card");

    cards.forEach(card => {
        card.addEventListener("mouseenter", () => {
            card.style.transform = "scale(1.05)";
            card.style.transition = "0.3s";
        });

        card.addEventListener("mouseleave", () => {
            card.style.transform = "scale(1)";
        });
    });
});
