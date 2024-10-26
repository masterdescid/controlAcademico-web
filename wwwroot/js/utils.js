function formatoTablaResponsive() {
    $("#tblResponsive").DataTable({
        ordering: true,
        order: [[0, "desc"]],
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ de registros.",
            "infoEmpty": "Mostrando 0 a 0 de 0 registros.",
            "infoFiltered": "(Filtrado de _MAX_ total de registros)",
            "infoPostFix": "",
            "thousands": "",
            "lengthMenu": "Mostrar _MENU_ de registros",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior",
            }
        }
    })
}

async function eliminarRegistro(codigoRegistro, nombreRegistro, controller, accion) {

    Swal.fire({
        title: "Eliminar registro",
        text: `¿Desea eliminar el registro ${nombreRegistro}?`,
        icon: "question",
        showCancelButton: true,
        cancelButtonText: "No",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí",
        showLoaderOnConfirm: true,
        preConfirm: async (proceso) => {

            try {
                const urlOrigen = window.location.origin;
                //TODO: Corregir el como se arma el link.
                const urlEliminar = `${urlOrigen}/${controller}/${accion}?codigoRegistro=${codigoRegistro}`;
                const response = await fetch(urlEliminar, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        //'Authorization': 'Bearer tuTokenAqui' // Si necesitas autenticación
                    }
                });

                if (!response.ok) {
                    console.log(`RESPUESTA: ${response.text}`);
                    return Swal.showValidationMessage(`${JSON.stringify(await response.json())}`);
                }

                return response.text();

            } catch (error) {
                Swal.showValidationMessage(`ERROR: ${error}`);
            }
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then((result) => {
        if (result.isConfirmed) {
            try {
                //TODO: Hace falta validar el código de respuesta personalizado que retorna para mostrar el valor.
                console.log(`CONFIRMADO: ${result.value}`);
                toast('success', 3000, 'top-end', `El registro -${nombreRegistro}- fue eliminado satisfactoriamente.`)

                setTimeout(function () {
                    location.reload();
                }, 3000);

            } catch (error) {
                console.error('Error en la solicitud:', error);
                return Swal.showValidationMessage("Error al procesar la solicitud.");
            }
        } else {
            toast('info', 3000, 'top-end', `El registro -${nombreRegistro}- NO se eliminó.`)
        }
    });
}

function toast(icono, tiempo, posicion, titulo) {
    const Toast = Swal.mixin({
        toast: true,
        position: posicion,
        showConfirmButton: false,
        timer: tiempo,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        }
    });
    Toast.fire({
        icon: icono,
        title: `${titulo}`
    });
}


function toastError(icono, tiempo, titulo, confirmButtonText, errorDetails) {
    const mensajeAdicional = 'Mensaje adicional que se mostrará al hacer clic en "Ver Más".';
    const swalOptions = {
        icon: icono,
        title: `<b>${titulo}</b>`,
        html: `<br>${errorDetails}<br><br><button class="swal2-confirm swal2-styled aceptar-btn" aria-label="" style="display: inline-block; background-color: #0064ff; color: #fff;">${confirmButtonText}</button><br><div style="margin-top: 10px;"><hr></div><div class="ver-mas" style="display: inline-block; margin-top: 10px; cursor: pointer; color: #0064ff;">Ver Más</div>`,
        showConfirmButton: false,
        showCancelButton: false,
        didOpen: (toast) => {
            toast.querySelector('.swal2-confirm').addEventListener('click', () => {
                Swal.close();
            });

            const verMasBtn = toast.querySelector('.ver-mas');
            verMasBtn.addEventListener('click', () => {
                Swal.update({
                    title: 'Mensaje Adicional',
                    text: mensajeAdicional,
                    confirmButtonText: 'Cerrar',
                    showConfirmButton: true,
                });
            });
        },
    };
    Swal.fire(swalOptions);
    setTimeout(() => {
        Swal.close();
    }, tiempo);
}
