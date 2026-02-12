using Crud_Inventario.Data.Infrastructure;
using Crud_Inventario.Data.Interfaces;
using Crud_Inventario.Models;
using Crud_Inventario.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Crud_Inventario.Data.Repositories
{
    public class MovimientosRepository : IMovimientoRepository
    {
        private readonly DatabaseConnection _dbconnection;

        public MovimientosRepository()
        {
            _dbconnection = new DatabaseConnection();
        }

        public async Task<(bool success, string mensaje)> ActualizarMovimientoAsync(MovInventario movimiento)
        {
            try
            {
                using (SqlConnection conn = _dbconnection.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_MOVIMIENTO", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@COD_CIA", movimiento.COD_CIA);
                        cmd.Parameters.AddWithValue("@COMPANIA_VENTA_3", movimiento.COMPANIA_VENTA_3);
                        cmd.Parameters.AddWithValue("@ALMACEN_VENTA", movimiento.ALMACEN_VENTA);
                        cmd.Parameters.AddWithValue("@TIPO_MOVIMIENTO", movimiento.TIPO_MOVIMIENTO);
                        cmd.Parameters.AddWithValue("@TIPO_DOCUMENTO", movimiento.TIPO_DOCUMENTO);
                        cmd.Parameters.AddWithValue("@NRO_DOCUMENTO", movimiento.NRO_DOCUMENTO);
                        cmd.Parameters.AddWithValue("@COD_ITEM_2", movimiento.COD_ITEM_2);
                        cmd.Parameters.AddWithValue("@PROVEEDOR", (object)movimiento.PROVEEDOR ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ALMACEN_DESTINO", (object)movimiento.ALMACEN_DESTINO ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CANTIDAD", (object)movimiento.CANTIDAD ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOC_REF_1", (object)movimiento.DOC_REF_1 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOC_REF_2", (object)movimiento.DOC_REF_2 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOC_REF_3", (object)movimiento.DOC_REF_3 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOC_REF_4", (object)movimiento.DOC_REF_4 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOC_REF_5", (object)movimiento.DOC_REF_5 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FECHA_TRANSACCION", (object)movimiento.FECHA_TRANSACCION ?? DBNull.Value);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int result = Convert.ToInt32(reader["Result"]);
                                string mensaje = reader["Message"].ToString();
                                return (result == 1, mensaje);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error al actualizar movimiento", ex);
                return (false, $"Error: {ex.Message}");
            }

            return (false, "Error al ejecutar el procedimiento");
        }
        public async Task<List<MovInventario>> ConsultarMovimientosAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoMovimiento = null, string nroDocumento = null)
        {
            var movimientos = new List<MovInventario>();

            try
            {
                using (SqlConnection conn = _dbconnection.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("SP_CONSULTAR_MOVIMIENTOS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = fechaInicio.HasValue
                                                        ? fechaInicio.Value.Date : (object)DBNull.Value;
                        cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = fechaFin.HasValue
                                                        ? fechaFin.Value.Date : (object)DBNull.Value;
                        cmd.Parameters.AddWithValue("@TipoMovimiento", string.IsNullOrWhiteSpace(tipoMovimiento)
                            ? (object)DBNull.Value : tipoMovimiento);
                        cmd.Parameters.AddWithValue("@NroDocumento", string.IsNullOrWhiteSpace(nroDocumento)
                            ? (object)DBNull.Value : nroDocumento);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                movimientos.Add(new MovInventario
                                {
                                    COD_CIA = reader["COD_CIA"].ToString(),
                                    COMPANIA_VENTA_3 = reader["COMPANIA_VENTA_3"].ToString(),
                                    ALMACEN_VENTA = reader["ALMACEN_VENTA"].ToString(),
                                    TIPO_MOVIMIENTO = reader["TIPO_MOVIMIENTO"].ToString(),
                                    TIPO_DOCUMENTO = reader["TIPO_DOCUMENTO"].ToString(),
                                    NRO_DOCUMENTO = reader["NRO_DOCUMENTO"].ToString(),
                                    COD_ITEM_2 = reader["COD_ITEM_2"].ToString(),
                                    PROVEEDOR = reader["PROVEEDOR"] != DBNull.Value ? reader["PROVEEDOR"].ToString() : null,
                                    ALMACEN_DESTINO = reader["ALMACEN_DESTINO"] != DBNull.Value ? reader["ALMACEN_DESTINO"].ToString() : null,
                                    CANTIDAD = reader["CANTIDAD"] != DBNull.Value ? (int?)reader["CANTIDAD"] : null,
                                    DOC_REF_1 = reader["DOC_REF_1"] != DBNull.Value ? reader["DOC_REF_1"].ToString() : null,
                                    DOC_REF_2 = reader["DOC_REF_2"] != DBNull.Value ? reader["DOC_REF_2"].ToString() : null,
                                    DOC_REF_3 = reader["DOC_REF_3"] != DBNull.Value ? reader["DOC_REF_3"].ToString() : null,
                                    DOC_REF_4 = reader["DOC_REF_4"] != DBNull.Value ? reader["DOC_REF_4"].ToString() : null,
                                    DOC_REF_5 = reader["DOC_REF_5"] != DBNull.Value ? reader["DOC_REF_5"].ToString() : null,
                                    FECHA_TRANSACCION = reader["FECHA_TRANSACCION"] != DBNull.Value ? (DateTime?)reader["FECHA_TRANSACCION"] : null
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error al consultar movimientos", ex);
            }

            return movimientos;
        }
        public async Task<(bool success, string mensaje)> EliminarMovimientoAsync(string codCia, string companiaVenta, string almacenVenta, string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem)
        {
            try
            {
                using (SqlConnection con = _dbconnection.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ELIMINAR_MOVIMIENTO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_CIA", codCia);
                        cmd.Parameters.AddWithValue("@COMPANIA_VENTA_3", companiaVenta);
                        cmd.Parameters.AddWithValue("@ALMACEN_VENTA", almacenVenta);
                        cmd.Parameters.AddWithValue("@TIPO_MOVIMIENTO", tipoMovimiento);
                        cmd.Parameters.AddWithValue("@TIPO_DOCUMENTO", tipoDocumento);
                        cmd.Parameters.AddWithValue("@NRO_DOCUMENTO", nroDocumento);
                        cmd.Parameters.AddWithValue("@COD_ITEM_2", codItem);

                        await con.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int result = Convert.ToInt32(reader["Result"]);
                                string mensaje = reader["Message"].ToString();
                                return (result == 1, mensaje);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Erro al eliminar movimiento", ex);
                return (false, $"Error: {ex.Message}");
            }

            return (false, "Error al ejecutar el procedimiento");
        }
        public async Task<(bool success, string mensaje)> InsertarMovimientoAsync(MovInventario movimiento)
        {
            try
            {
                using (SqlConnection conn = _dbconnection.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("SP_INSERTAR_MOVIMIENTO", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_CIA", movimiento.COD_CIA);
                        cmd.Parameters.AddWithValue("@COMPANIA_VENTA_3", movimiento.COMPANIA_VENTA_3);
                        cmd.Parameters.AddWithValue("@ALMACEN_VENTA", movimiento.ALMACEN_VENTA);
                        cmd.Parameters.AddWithValue("@TIPO_MOVIMIENTO", movimiento.TIPO_MOVIMIENTO);
                        cmd.Parameters.AddWithValue("@TIPO_DOCUMENTO", movimiento.TIPO_DOCUMENTO);
                        cmd.Parameters.AddWithValue("@NRO_DOCUMENTO", movimiento.NRO_DOCUMENTO);
                        cmd.Parameters.AddWithValue("@COD_ITEM_2", movimiento.COD_ITEM_2);
                        cmd.Parameters.AddWithValue("@PROVEEDOR", (object)movimiento.PROVEEDOR ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ALMACEN_DESTINO", (object)movimiento.ALMACEN_DESTINO ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CANTIDAD", (object)movimiento.CANTIDAD ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOC_REF_1", (object)movimiento.DOC_REF_1 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOC_REF_2", (object)movimiento.DOC_REF_2 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOC_REF_3", (object)movimiento.DOC_REF_3 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOC_REF_4", (object)movimiento.DOC_REF_4 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOC_REF_5", (object)movimiento.DOC_REF_5 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FECHA_TRANSACCION", (object)movimiento.FECHA_TRANSACCION ?? DBNull.Value);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int result = Convert.ToInt32(reader["Result"]);
                                string mensaje = reader["Message"].ToString();
                                return (result == 1, mensaje);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error al insertar movimiento", ex);
                return (false, $"Error: {ex.Message}");
            }

            return (false, "Error al ejecutar el procedimiento");
        }
        public async Task<MovInventario> ObtenerPorIdAsync(string codCia, string companiaVenta, string almacenVenta, string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem)
        {
            try
            {
                var movimientos = await ConsultarMovimientosAsync();
                return movimientos.Find(m =>
                    m.COD_CIA == codCia &&
                    m.COMPANIA_VENTA_3 == companiaVenta &&
                    m.ALMACEN_VENTA == almacenVenta &&
                    m.TIPO_MOVIMIENTO == tipoMovimiento &&
                    m.TIPO_DOCUMENTO == tipoDocumento &&
                    m.NRO_DOCUMENTO == nroDocumento &&
                    m.COD_ITEM_2 == codItem
                );
            }
            catch (Exception ex)
            {
                Logger.LogError("Error al obtener movimiento", ex);
                return null;
            }
        }
    }
}