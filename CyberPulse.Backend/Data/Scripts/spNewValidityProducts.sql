-- Suponiendo que su tabla de productos se llama 'Products'

CREATE PROCEDURE Inve.ProductsWithNewValidity
    @Percentage decimal(6,3),
    @ErrorCode varchar(6) OUTPUT  -- Parámetro de salida para manejar errores
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ValidityId INT = 0;

    DECLARE @ValidityOldId INT = 0;

    SET @ErrorCode = 'Ok'; -- Inicializar sin errores

    -- 1. Verificar si Hay una vigencia activa
    IF NOT EXISTS (SELECT 1 FROM Inve.Validities where StatuId=1)
    BEGIN
        SET @ErrorCode = 'ERR014'; -- Código de error: 'No se encontraron vigencia activa
        RETURN;
    END

    SELECT @ValidityId=Id FROM Inve.Validities WHERE StatuId=1
    -- 2. Verificar si ya existen productos para la nueva vigencia
    IF EXISTS (SELECT 1 FROM Inve.ProductCurrentValues WHERE ValidityId = @ValidityId)
    BEGIN
        SET @ErrorCode = 'ERR015'; -- Código de error: 'Ya existen productos para este año'
        RETURN;
    END

    SET @ValidityOldId=@ValidityId-1

    -- 3. Verificar si hay productos en la vigencia anterior para duplicar
    IF NOT EXISTS (SELECT 1 FROM Inve.ProductCurrentValues WHERE ValidityId = @ValidityOldId)
    BEGIN
        SET @ErrorCode = 'ERR016'; -- Código de error: 'No se encontraron productos para el año anterior'
        RETURN;
    END

    -- 4. Duplicación e Incremento del 13%
    BEGIN TRY
        INSERT INTO Inve.ProductCurrentValues (ValidityId, ProductId, IvaId, Worth)
        SELECT
            @ValidityId,
            P.ProductId,
            P.IvaId,
            P.Worth+(P.Worth * @Percentage)
        FROM
            Inve.ProductCurrentValues as P, Inve.Validities as V
        WHERE
            P.ValidityId=V.Id AND
            V.Value = @ValidityOldId
        ORDER BY P.Id;
        -- @ErrorCode = 0 (Éxito), se mantiene
    END TRY
    BEGIN CATCH
        SET @ErrorCode = 'ERR017'; -- Código de error: 'Error interno de SQL'
    END CATCH
END