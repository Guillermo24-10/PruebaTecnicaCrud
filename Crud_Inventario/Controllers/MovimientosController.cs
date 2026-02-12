using Crud_Inventario.Data.Interfaces;
using Crud_Inventario.Data.Repositories;
using Crud_Inventario.Models;
using Crud_Inventario.Utilities;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Crud_Inventario.Controllers
{
    public class MovimientosController : Controller
    {
        private readonly IMovimientoRepository _movimientoRepository;

        public MovimientosController()
        {
            _movimientoRepository = new MovimientosRepository(); 
        }
        public MovimientosController(IMovimientoRepository movimientoRepository)
        {
            _movimientoRepository = movimientoRepository;
        }

        public async Task<ActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin,
            string tipoMovimiento, string nroDocumento)
        {
            try
            {
                var movimientos = await _movimientoRepository.ConsultarMovimientosAsync(fechaInicio, fechaFin, tipoMovimiento, nroDocumento);

                ViewBag.FechaInicio = fechaInicio;
                ViewBag.FechaFin = fechaFin;
                ViewBag.TipoMovimiento = tipoMovimiento;
                ViewBag.NroDocumento = nroDocumento;

                return View(movimientos);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error en Index-Movimiento", ex);
                TempData["Error"] = "Error al cargar los movimientos";
                return View();
            }
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(MovInventario movimiento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (success, mensaje) = await _movimientoRepository.InsertarMovimientoAsync(movimiento);
                    if (success)
                    {
                        TempData["Success"] = mensaje;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = mensaje;
                    }
                }

                return View(movimiento);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error en Crear-POST", ex);
                TempData["Error"] = "Error al crear el movimiento";
                return View(movimiento);
            }
        }

        public async Task<ActionResult> Editar(string codCia, string companiaVenta, string almacenVenta,
            string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem)
        {
            try
            {
                var movimiento = await _movimientoRepository.ObtenerPorIdAsync(
                    codCia, companiaVenta, almacenVenta, tipoMovimiento,
                    tipoDocumento, nroDocumento, codItem);

                if (movimiento == null)
                {
                    TempData["Error"] = "Movimiento no encontrado";
                    return RedirectToAction("Index");
                }

                return View(movimiento);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error en Editar-GET", ex);
                TempData["Error"] = "Error al cargar el movimiento";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(MovInventario movimiento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (success, mensaje) = await _movimientoRepository.ActualizarMovimientoAsync(movimiento);

                    if (success)
                    {
                        TempData["Success"] = mensaje;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = mensaje;
                    }
                }

                return View(movimiento);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error en Editar-POST", ex);
                TempData["Error"] = "Error al actualizar el movimiento";
                return View(movimiento);
            }
        }

        public async Task<ActionResult> Eliminar(string codCia, string companiaVenta, string almacenVenta,
            string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem)
        {
            try
            {
                var movimiento = await _movimientoRepository.ObtenerPorIdAsync(
                    codCia, companiaVenta, almacenVenta, tipoMovimiento,
                    tipoDocumento, nroDocumento, codItem);

                if(movimiento == null)
                {
                    TempData["Error"] = "Movimiento no encontrado";
                    return RedirectToAction("Index");
                }

                return View(movimiento);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error en Eliminar GET", ex);
                TempData["Error"] = "Error al cargar el movimiento";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmarEliminacion(string codCia, string companiaVenta, string almacenVenta,
            string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem)
        {
            try
            {
                var (success, mensaje) = await _movimientoRepository.EliminarMovimientoAsync(
                    codCia, companiaVenta, almacenVenta, tipoMovimiento,
                    tipoDocumento, nroDocumento, codItem);

                if (success)
                {
                    TempData["Success"] = mensaje;
                }
                else
                {
                    TempData["Error"] = mensaje;
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error en Eliminar POST", ex);
                TempData["Error"] = "Error al eliminar el movimiento";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Detalle(string codCia, string companiaVenta, string almacenVenta,
            string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem)
        {
            try
            {
                var movimiento = await _movimientoRepository.ObtenerPorIdAsync(
                    codCia, companiaVenta, almacenVenta, tipoMovimiento,
                    tipoDocumento, nroDocumento, codItem);

                if (movimiento == null)
                {
                    TempData["Error"] = "Movimiento no encontrado";
                    return RedirectToAction("Index");
                }

                return View(movimiento);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error en Detalle", ex);
                TempData["Error"] = "Error al cargar el detalle";
                return RedirectToAction("Index");
            }
        }
    }
}