$(document).ready(function () {

    $("#btnConsultar").click(function () {

        var id = $("#ddlCompras").val();

        if (!id || id === "") {
            alert("Seleccione una compra válida.");
            return;
        }

        $.ajax({
            url: "/Abonos/ObtenerSaldo",
            data: { id: id },
            success: function (data) {
                $("#SaldoAnterior").val(data);
            },
            error: function () {
                alert("No se pudo obtener el saldo. Intente nuevamente.");
            }
        });

    });

});