// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showFullscreenImage(imgSrc) {
    var overlay = document.getElementById('fullscreenOverlay');
    var img = document.getElementById('fullscreenImage');
    img.src = imgSrc;
    overlay.classList.add('show');
}

function hideFullscreenImage() {
    var overlay = document.getElementById('fullscreenOverlay');
    overlay.classList.remove('show');
}

function incrementQuantity() {
    var quantityInput = document.getElementById('quantity');
    quantityInput.value = parseInt(quantityInput.value) + 1;
}

function decrementQuantity() {
    var quantityInput = document.getElementById('quantity');
    if (quantityInput.value > 1) {
        quantityInput.value = parseInt(quantityInput.value) - 1;
    }
}