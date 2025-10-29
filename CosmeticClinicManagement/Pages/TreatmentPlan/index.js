$(function () {
    var editModal = new abp.ModalManager(abp.appPath + 'TreatmentPlan/EditTreatmentPlanModal');
    var createSessionModal = new abp.ModalManager(abp.appPath + 'Sessions/CreateSessionModal');

    var l = abp.localization.getResource('CosmeticClinicManagement');
    var dataTable = $('#TreatmentPlansTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[0, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(
                cosmeticClinicManagement.services.implementation.treatmentPlan.getList),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    confirmMessage: function (data) {
                                        return l('TreatmentPlanDeletionConfirmationMessage',
                                            data.record.name);
                                    },
                                    action: function (data) {
                                        cosmeticClinicManagement.services.implementation.treatmentPlan.delete(data.record.id).then(function () {
                                                abp.notify.info(l('SuccessfullyDeleted'));
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                                
                            ]
                    }
                },
                
                {
                    title: l('DoctorId'),
                    data: "doctorId"
                },
                {
                    title: l('PatientId'),
                    data: "patientId",
                    orderable: false
                },
             
                {
                    title: l('Status'),
                    data: "status",
                    render: function (data) {
                        return l('Enum:Status:' + data);
                    }
                },
                {
                    title: l('Sessions'),
                    data: "sessions"
                    //rowAction: {
                    //        [
                    //            {
                                 
                    //                action: function (data) {
                    //                    editModal.open({ id: data.record.id });
                    //                }
                    //            }
                    //        ]
                    //}
                }, {
                    title: l('Sessions'),
                    rowAction: {
                        items:
                            [
                                
                                {
                                    text: l('New Session'),
                                    action: function (data) {
                                        createSessionModal.open({ planId: data.record.id });
                                    }
                                }
                            ]
                    }
                },


               

            ]
        })
    );
editModal.onResult(function () {
    dataTable.ajax.reload();
});
    createSessionModal.onResult(function () {
        dataTable.ajax.reload();
    });
    var createModal = new abp.ModalManager(abp.appPath +
        'TreatmentPlan/CreateTreatmentPlanModal');
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });
    $('#NewTreatmentPlanButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
