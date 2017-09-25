﻿using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabajoFinalApp.Modelo
{
    public class PedidoVentaDetalle
    {
        [PrimaryKey, AutoIncrement]
        public int IdPedidoVentaDetalle { get; set; }
        public int Cantidad { get; set; }
        public double SubTotal { get; set; }
        public double PorcentajeDescuento { get; set; }
        public double Total { get; set; }

        [ForeignKey(typeof(PedidoVenta))]
        public int IdPedidoVenta { get; set; }

        [ForeignKey(typeof(Articulo))]
        public int IdArticulo { get; set; }

        //Agregado para que se vea en la lista
        public string Articulo { get; set; }
        public double PrecioUnitario { get; set; }

        public PedidoVentaDetalle() { }        
     
         
    }
}
