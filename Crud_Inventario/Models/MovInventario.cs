using System;
using System.ComponentModel.DataAnnotations;

namespace Crud_Inventario.Models
{
    public class MovInventario
    {
        [Display(Name = "Código CIA")]
        [Required(ErrorMessage = "El código de compañía es requerido")]
        [StringLength(5)]
        public string COD_CIA { get; set; }

        [Display(Name = "Compañía Venta")]
        [Required(ErrorMessage = "La compañía de venta es requerida")]
        [StringLength(5)]
        public string COMPANIA_VENTA_3 { get; set; }

        [Display(Name = "Almacén Venta")]
        [Required(ErrorMessage = "El almacén de venta es requerido")]
        [StringLength(10)]
        public string ALMACEN_VENTA { get; set; }

        [Display(Name = "Tipo Movimiento")]
        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        [StringLength(2)]
        public string TIPO_MOVIMIENTO { get; set; }

        [Display(Name = "Tipo Documento")]
        [Required(ErrorMessage = "El tipo de documento es requerido")]
        [StringLength(2)]
        public string TIPO_DOCUMENTO { get; set; }

        [Display(Name = "Nro. Documento")]
        [Required(ErrorMessage = "El número de documento es requerido")]
        [StringLength(50)]
        public string NRO_DOCUMENTO { get; set; }

        [Display(Name = "Código Item")]
        [Required(ErrorMessage = "El código de item es requerido")]
        [StringLength(50)]
        public string COD_ITEM_2 { get; set; }

        [Display(Name = "Proveedor")]
        [StringLength(100)]
        public string PROVEEDOR { get; set; }

        [Display(Name = "Almacén Destino")]
        [StringLength(50)]
        public string ALMACEN_DESTINO { get; set; }

        [Display(Name = "Cantidad")]
        public int? CANTIDAD { get; set; }

        [Display(Name = "Doc. Ref. 1")]
        [StringLength(50)]
        public string DOC_REF_1 { get; set; }

        [Display(Name = "Doc. Ref. 2")]
        [StringLength(50)]
        public string DOC_REF_2 { get; set; }

        [Display(Name = "Doc. Ref. 3")]
        [StringLength(50)]
        public string DOC_REF_3 { get; set; }

        [Display(Name = "Doc. Ref. 4")]
        [StringLength(50)]
        public string DOC_REF_4 { get; set; }

        [Display(Name = "Doc. Ref. 5")]
        [StringLength(50)]
        public string DOC_REF_5 { get; set; }

        [Display(Name = "Fecha Transacción")]
        [DataType(DataType.Date)]
        public DateTime? FECHA_TRANSACCION { get; set; }
    }
}