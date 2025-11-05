function initSessionsTable(planId) {
    // Destroy if already exists (fixes "sorting not defined" on re-open)
    var $table = $('#SessionsForPlanTable');
    if ($.fn.DataTable.isDataTable($table)) {
        $table.DataTable().destroy();
        $table.empty();
    }
    var editModal = new abp.ModalManager(abp.appPath + 'Sessions/EditSessionModal');
    //var planId = new URLSearchParams(window.location.search).get('planId');

    var l = abp.localization.getResource('CosmeticClinicManagement');
    var dataTable = $table.DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[0, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(
                cosmeticClinicManagement.services.implementation.treatmentPlan.getSessions, { planId: planId }),
            columnDefs: [
                /* TODO: Column definitions */
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function (data) {
                                    return l('ProductDeletionConfirmationMessage',
                                        data.record.name);
                                },
                                action: function (data) {
                                    cosmeticClinicManagement.services.implementation.session.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('SuccessfullyDeleted'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }

                        ]
                    }
                },
                {
                    title: l('SessionDate'),
                    data: "sessionDate",
                    dataFormat: 'date'
                },
                {
                    title: l('UsedMaterials'),
                    data: "usedMaterials",
                    orderable: false
                },
                {
                    title: l('Notes'),
                    data: "notes"
                },
                {
                    title: l('SessionStatus'),
                    data: "sessionStatus",
                    render: function (data) {
                        return l('Enum:SessionStatus:' + data);
                    }

                }


            ]
        })
    );
    editModal.onResult(function () {
        dataTable.ajax.reload();
    });
    var createModal = new abp.ModalManager(abp.appPath +
        'Sessions/CreateSessionModal');
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });
    $('#NewSessionButton').off('click').on('click', function (e) {
        e.preventDefault();
        createSessionModal.open({ planId: planId });
    });
}
$(function () {
    var planId = new URLSearchParams(window.location.search).get('planId');
    if (planId) 
        initSessionsTable(planId);
    
    }
);
