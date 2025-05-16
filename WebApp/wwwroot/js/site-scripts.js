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

    // GLOBAL delete trigger — Secure with AntiForgeryToken
    document.querySelectorAll("[data-delete-url]").forEach(btn => {
        btn.addEventListener("click", function (e) {
            e.preventDefault();

            const deleteUrl = this.dataset.deleteUrl;
            const confirmText = this.dataset.confirmMessage || "Are you sure you want to delete this item?";
            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

            showConfirmation(confirmText, () => {
                // Step 1: POST to DeleteCheck
                fetch(deleteUrl, {
                    method: "POST",
                    headers: {
                        "RequestVerificationToken": token
                    }
                })
                    .then(r => r.json())
                    .then(res => {
                        if (res.requiresInactive) {
                            // Step 2: Ask to set inactive
                            showConfirmation(res.message, () => {
                                fetch(res.setInactiveUrl, {
                                    method: "POST",
                                    headers: {
                                        "RequestVerificationToken": token
                                    }
                                })
                                    .then(r => r.json())
                                    .then(result => {
                                        if (result.success) {
                                            showFlash(result.message, result.type);
                                            setTimeout(() => location.reload(), 2000);
                                        } else {
                                            showFlash(result.message || "An error occurred.", result.type || "error");
                                        }
                                    });
                            });
                        } else if (res.success && res.redirectUrl) {
                            showFlash(res.message, res.type || "success");
                            setTimeout(() => window.location.href = res.redirectUrl, 2000);
                        } else if (res.success && res.message) {
                            showFlash(res.message, res.type || "success");
                            setTimeout(() => location.reload(), 2000);
                        } else {
                            showFlash(res.message || "An error occurred.", res.type || "error");
                        }
                    })
                    .catch(() => {
                        showFlash("Request failed. Please try again.", "error");
                    });
            });
        });
    });

    // GLOBAL Set Active trigger
    document.querySelectorAll("[data-set-active-url]").forEach(btn => {
        btn.addEventListener("click", function (e) {
            e.preventDefault();

            const url = this.dataset.setActiveUrl;
            const confirmText = this.dataset.confirmMessage || "Are you sure you want to activate this item?";
            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

            showConfirmation(confirmText, () => {
                fetch(url, {
                    method: "POST",
                    headers: {
                        "RequestVerificationToken": token
                    }
                })
                    .then(r => r.json())
                    .then(res => {
                        if (res.success) {
                            showFlash(res.message || "Item activated.", res.type || "success");
                            setTimeout(() => location.reload(), 2000);
                        } else {
                            showFlash(res.message || "Failed to activate item.", res.type || "error");
                        }
                    })
                    .catch(() => {
                        showFlash("Activation request failed.", "error");
                    });
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
