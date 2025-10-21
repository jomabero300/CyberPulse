function displayPdf(pdfBytes) {
    // Convert byte array to Blob
    const blob = new Blob([new Uint8Array(pdfBytes)], { type: 'application/pdf' });
    const url = URL.createObjectURL(blob);
    window.open(url, '_blank');
    // Remember to revoke the URL when done to free memory
    // URL.revokeObjectURL(url); // Can be done after loading
    setTimeout(() => URL.revokeObjectURL(url), 10000);

}