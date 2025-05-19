function setRating(rate) {
    document.getElementById("Rate").value = rate;
    updateStars(rate);
}

function updateStars(rate) {
    const stars = document.querySelectorAll('.rating-stars .fa-star');
    stars.forEach((star, index) => {
        if (index < rate) {
            star.classList.remove('text-muted');
            star.classList.add('text-success');
        }
        else {
            star.classList.remove('text-success');
            star.classList.add('text-muted');
        }
    });
}

// Ensure stars are visually correct on page load
document.addEventListener('DOMContentLoaded', () => {
    const currentRating = parseInt(document.getElementById("Rate").value) || 0;
    updateStars(currentRating);
});

function validateFeedbackForm() {
    const rate = parseFloat(document.getElementById("Rate").value) || 0;
    const note = document.querySelector("textarea[name='Note']").value.trim();

    if (note.length < 3) {
        showFlash("Note must be at least 3 characters long.", "error");
        return false;
    }

    if (rate < 1) {
        showFlash("Please select a rating before submitting.", "error");
        return false;
    }

    return true;
}