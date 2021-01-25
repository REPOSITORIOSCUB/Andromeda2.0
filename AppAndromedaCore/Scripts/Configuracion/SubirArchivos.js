$("#btnSubirArchivo").click(function (eve) {
    $("#modal-content").load("/Archivos/Index?accion=Create")
});

$("#btnSubirArchivoEdit").click(function (eve) {   
    var id = $("#id").val();
    $("#modal-content").load("/Archivos/Index?id="+id+"&accion=Edit")
});