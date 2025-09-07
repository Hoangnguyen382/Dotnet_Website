window.showModal = (modalId) => {
    const modal = new bootstrap.Modal(document.getElementById(modalId));
    modal.show();
};
window.hideModal = (modalId) => {
    const modal = bootstrap.Modal.getInstance(document.getElementById(modalId));
    modal.hide();
};