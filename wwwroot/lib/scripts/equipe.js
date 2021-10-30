$(document).ready(function () {


    $("BatalhaoId").change(function () {

        var value = $("#BatalhaoId option:selected").val();

        if (value != " " || value != !undefined) {
            ListarCompanhia(value);
        }
    })
})

function ListarCompanhia(value) {

    var url = "/Equipe/ListarCompanhia";
    var data = { sigla: value };

    $("#CompanhiaId").empty();

    $.ajax({

        url: url,
        type : "GET",
        datatype : "json",
        data: data
    }).done(function (data) {
        if (data.Resultado.length > 0) {
            var dadosGrid = data.Resultado;

            $("#CompanhiaId").append('<option value= "">Selecionar </option>');

            $.each(dadosGrid, function (indice, item) {

                var opt = "";

                opt = '<option value="' + item["CompanhiaId"] + '">' + item["CompanhiaNome"] + '</option>';

                $("#CompanhiaId").append(opt);


            });
        }

    })
}
