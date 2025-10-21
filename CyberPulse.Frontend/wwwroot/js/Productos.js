window.mostrarPdfEnNuevaPestana = (bytes) => {
    // Convertir los bytes a un Uint8Array
    const byteArray = new Uint8Array(bytes);

    // Crear un blob
    const blob = new Blob([byteArray], { type: 'application/pdf' });

    // Crear una URL y abrirla en una nueva pestaña
    const url = URL.createObjectURL(blob);
    window.open(url, '_blank');

    // Revocar la URL después de un tiempo
    setTimeout(() => URL.revokeObjectURL(url), 10000);
};