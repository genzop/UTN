﻿using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabajoFinalApp.Modelo;
using Xamarin.Forms;

namespace TrabajoFinalApp.Controladores
{
    public class ControladorPedidoVenta : IDisposable
    {
        private SQLiteConnection conexion;

        public ControladorPedidoVenta()
        {
            var config = DependencyService.Get<IConfig>();
            conexion = new SQLiteConnection(config.Plataforma, System.IO.Path.Combine(config.DirectorioDB, "PedidosVentas.db3"));
            conexion.CreateTable<PedidoVenta>();
        }

        public void Insert(PedidoVenta ped)
        {
            conexion.Insert(ped);
        }

        public void Update(PedidoVenta ped)
        {
            conexion.Update(ped);
        }

        public void Delete(PedidoVenta ped)
        {
            conexion.Delete(ped);
        }

        public void DeleteAll()
        {
            conexion.DeleteAll<PedidoVenta>();
        }

        public PedidoVenta FindById(int id)
        {
            var pedidoVenta = (from ped in conexion.Table<PedidoVenta>()
                               where ped.IdPedidoVenta == id
                               select ped).FirstOrDefault();
            return pedidoVenta;
        }

        public List<PedidoVenta> ShowAll()
        {
            var pedidoVentas = (from ped in conexion.Table<PedidoVenta>()
                                orderby ped.IdPedidoVenta
                                select ped).ToList();
            return pedidoVentas;
        }

        public void Dispose()
        {
            conexion.Dispose();
        }

    }
}