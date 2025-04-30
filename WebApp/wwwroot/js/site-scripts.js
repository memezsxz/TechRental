function previewImage(event) {
    const file = event.target.files[0];
    const uploadBox = document.querySelector('.upload-box');
    const uploadtxt = document.querySelector('.upload-text');

    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            uploadBox.style.backgroundImage = `url('${e.target.result}')`;
            uploadBox.style.backgroundSize = 'cover';
            uploadBox.style.backgroundPosition = 'center';
            uploadtxt.innerHTML = ''; // Remove the "Add" text
        };
        reader.readAsDataURL(file);
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const priceInput = document.querySelector("input[name='RentalPricePerDay']");
    if (priceInput) {
        priceInput.addEventListener("input", function (e) {
            this.value = this.value.replace(/[^0-9.]/g, '');
        });
    }
});

document.addEventListener("DOMContentLoaded", function () {
    const flashMessage = document.getElementById("tempDataMessage")?.value;
    const flashType = document.getElementById("tempDataMessageType")?.value;

    // Flash modal logic
    if (flashMessage && flashType) {
        showFlash(flashMessage, flashType);
    }

    // GLOBAL delete trigger
    document.querySelectorAll("[data-delete-url]").forEach(btn => {
        btn.addEventListener("click", function (e) {
            e.preventDefault();
            const deleteUrl = this.dataset.deleteUrl;
            const confirmText = this.dataset.confirmMessage || "Are you sure you want to delete this item?";
            showConfirmation(confirmText, () => {
                // Step 1: Ask backend if FK exists
                fetch(deleteUrl, { method: "POST" })
                    .then(r => r.json())
                    .then(res => {
                        if (res.requiresInactive) {
                            // Step 2: Ask user if they want to set inactive
                            showConfirmation(res.message, () => {
                                // Step 3: Proceed to inactivate
                                fetch(res.setInactiveUrl, { method: "POST" })
                                    .then(r => r.json())
                                    .then(result => {
                                        if (result.success) {
                                            showFlash(result.message, result.type);
                                            setTimeout(() => location.reload(), 2000);
                                        }
                                    });
                            });
                        } else {
                            location.reload(); // Direct delete was successful
                        }
                    });
            });
        });
    });

    function showFlash(message, type) {
        const iconMap = {
            success: "fas fa-check-circle text-success",
            error: "fas fa-times-circle text-danger",
            info: "fas fa-info-circle text-primary",
            warning: "fas fa-exclamation-circle text-warning"
        };

        document.getElementById("flashIcon").className = iconMap[type] || "fas fa-info-circle text-secondary";
        document.getElementById("flashText").textContent = message;

        const modal = new bootstrap.Modal(document.getElementById("flashMessageModal"));
        modal.show();
        setTimeout(() => modal.hide(), 2500);
    }

    function showConfirmation(message, onConfirm) {
        document.getElementById("confirmationText").textContent = message;
        const modal = new bootstrap.Modal(document.getElementById("confirmationModal"));
        modal.show();

        const confirmBtn = document.getElementById("confirmYes");
        const newBtn = confirmBtn.cloneNode(true);
        confirmBtn.parentNode.replaceChild(newBtn, confirmBtn);

        newBtn.addEventListener("click", () => {
            modal.hide();
            onConfirm();
        });
    }
});
