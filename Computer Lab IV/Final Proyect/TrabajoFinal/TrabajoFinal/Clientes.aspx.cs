﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Clientes : System.Web.UI.Page
{
    BaseDatosDataContext bd = new BaseDatosDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        //Valida que haya un usuario logueado
        if (Session["IdVendedor"] == null)
        {
            Response.Redirect("Ingresar.aspx");
        }
        else
        {            
            HyperLink clientes = (HyperLink)Master.FindControl("hlClientes");
            clientes.CssClass = "active";

            //Si el usuario no es Administrador, se ocultan algunas funcionalidades
            var usuario = (from vend in bd.Vendedors
                           where vend.IdVendedor == Convert.ToInt32(Session["IdVendedor"])
                           select vend).FirstOrDefault();

            if (!usuario.Administrador)
            {                
                grdClientes.Columns[7].Visible = false;
                grdClientes.Columns[8].Visible = false;
                imgAdd.Visible = false;                         
            }            
        }        
    }

    protected void imgEdit_Command(object sender, CommandEventArgs e)
    {
        Response.Redirect("EditarCliente.aspx?id=" + e.CommandArgument);
    }

    protected void imgDelete_Command(object sender, CommandEventArgs e)
    {
        try
        {
            //Verifica si el Cliente tiene Pedidos asociados
            int idSeleccionado = Convert.ToInt32(e.CommandArgument.ToString());
            bool tienePedidos = (from ped in bd.Pedidos
                                 where ped.IdCliente == idSeleccionado
                                 select ped).Any();

            if (tienePedidos)
            {                
                ScriptManager.RegisterClientScriptBlock(updatePanel, GetType(), "errorEliminarRubro", "alert('ERROR: El cliente no puede ser eliminado ya que aun tiene pedidos asociados a su cuenta')", true);
            }
            //Sino tiene ningun Pedido asociado, se elimina el Cliente y su Domicilio
            else
            {
                var temp = (from cli in bd.Clientes
                            where cli.IdCliente == idSeleccionado
                            select cli).Single();

                var tempDom = (from dom in bd.Domicilios
                               where dom.IdDomicilio == temp.IdDomicilio
                               select dom).Single();

                bd.Clientes.DeleteOnSubmit(temp);
                bd.Domicilios.DeleteOnSubmit(tempDom);
                bd.SubmitChanges();
                grdClientes.DataBind();
            }
        }
        catch (Exception) { }       
    }

    protected void imgFind_Click(object sender, ImageClickEventArgs e)
    {
        string query = "";
        if (txtBuscar.Text == "")
        {
            query = "SELECT Cliente.IdCliente, Cliente.RazonSocial, Cliente.Cuit, Cliente.Saldo, Domicilio.Calle + ' ' + CAST(Domicilio.Numero AS nvarchar) AS Calle, Localidad.Denominacion, Provincia.Denominacion, Pais.Denominacion FROM Cliente INNER JOIN Domicilio ON Cliente.IdDomicilio = Domicilio.IdDomicilio INNER JOIN Localidad ON Domicilio.IdLocalidad = Localidad.IdLocalidad INNER JOIN Provincia ON Localidad.IdProvincia = Provincia.IdProvincia INNER JOIN Pais ON Provincia.IdPais = Pais.IdPais";
        }
        else
        {
            query = "SELECT Cliente.IdCliente, Cliente.RazonSocial, Cliente.Cuit, Cliente.Saldo, Domicilio.Calle + ' ' + CAST(Domicilio.Numero AS nvarchar) AS Calle, Localidad.Denominacion, Provincia.Denominacion, Pais.Denominacion FROM Cliente INNER JOIN Domicilio ON Cliente.IdDomicilio = Domicilio.IdDomicilio INNER JOIN Localidad ON Domicilio.IdLocalidad = Localidad.IdLocalidad INNER JOIN Provincia ON Localidad.IdProvincia = Provincia.IdProvincia INNER JOIN Pais ON Provincia.IdPais = Pais.IdPais WHERE " + ddlBuscar.SelectedValue + " LIKE '%" + txtBuscar.Text + "%'";
        }  
        SqlDataSource1.SelectCommand = query;
        grdClientes.DataBind();
    }

    protected void imgAdd_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("EditarCliente.aspx");
    }

    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ReporteClientes.ashx?id=" + Session["IdVendedor"]);
    }
}