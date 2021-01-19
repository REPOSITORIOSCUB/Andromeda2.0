$("#btnBuscarPersona").click(function (eve) {
    //$("#modal-content").load("/Usuarios/BucarPersonaPopup")
    $("#modal-content").load("/Usuarios/GetPerPopup")
})

//cuando es una clase se pasa asi
//HTML
//< a href = "#personasModla" class="btnEditar btn btn-info btn-xs tmn" data - toggle="modal" data - id="@item.idetificadion" > <i class='fas fa-pencil-alt'></i></a >

//SCRIPT
//$(".btnEditar").click(function (eve) {
//    $('#modal-content').load("/Usuarios/Edit/" + $(this).data("id"));
//});

//