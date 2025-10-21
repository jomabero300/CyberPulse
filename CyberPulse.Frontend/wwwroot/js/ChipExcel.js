window.saveAsFile = (fileName, byteBase64) => {
    if (!fileName || !byteBase64) {
        console.warn('Parámetros inválidos para saveAsFile');
        return;
    }

    try {
        const link = document.createElement('a');
        link.href = "data:application/octet-stream;base64," + byteBase64;
        link.download = fileName;
        link.style.display = 'none';

        document.body.appendChild(link);
        link.click();

        // Usar remove() que es más moderno y seguro
        if (link && link.parentNode) {
            link.remove();
        }

    } catch (error) {
        console.error('Error en saveAsFile:', error);
    }
}


//window.saveAsFile = (fileName, byteBase64) => {
//    var link = document.createElement('a');
//    link.href = "data:application/octet-stream;base64," + byteBase64;
//    link.download = fileName;
//    document.body.appendChild(link);
//    link.click();
//    document.body.removeChild(link); 
//}