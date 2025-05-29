window.dialogInterop = {
    show: (id) => document.getElementById(id)?.showModal(),
    close: (id) => document.getElementById(id)?.close()
};
