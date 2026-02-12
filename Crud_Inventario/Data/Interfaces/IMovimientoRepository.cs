using Crud_Inventario.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crud_Inventario.Data.Interfaces
{
    public interface IMovimientoRepository
    {
        Task<List<MovInventario>> ConsultarMovimientosAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoMovimiento = null, string nroDocumento = null);

        Task<(bool success, string mensaje)> InsertarMovimientoAsync(MovInventario movimiento);

        Task<(bool success, string mensaje)> ActualizarMovimientoAsync(MovInventario movimiento);

        Task<(bool success, string mensaje)> EliminarMovimientoAsync(string codCia, string companiaVenta,
            string almacenVenta, string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem);
        Task<MovInventario> ObtenerPorIdAsync(string codCia, string companiaVenta, string almacenVenta,
            string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem);
    }
}
