$(function () {
    var editModal = new abp.ModalManager(abp.appPath + 'TreatmentPlan/EditTreatmentPlanModal');
    var createSessionModal = new abp.ModalManager(abp.appPath + 'Sessions/CreateSessionModal');
    var editSessionModal = new abp.ModalManager(abp.appPath + 'Sessions/EditSessionModal');
    var viewSessionModal = new abp.ModalManager(abp.appPath + 'Sessions/ViewSessionsModal');

    var l = abp.localization.getResource('CosmeticClinicManagement');
    var currentPlanId = null;

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
                                    return l('TreatmentPlanDeletionConfirmationMessage',
                                        data.record.patientFullName);
                                },
                                action: function (data) {
                                    cosmeticClinicManagement.services.implementation.treatmentPlan.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('SuccessfullyDeletedTreatmentPlan'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                    }
                },
                {
                    title: l('Doctor'),
                    data: "doctorName"
                },
                {
                    title: l('Patient'),
                    data: "patientFullName",
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
                    data: "sessions",
                    render: function (sessionsArray, type, row) {
                        var planId = row.id;
                        var sessionCount = sessionsArray ? sessionsArray.length : 0;

                        var viewSessionsButton = `<a href="javascript:void(0);" class="btn btn-sm btn-link view-sessions" data-plan-id="${planId}">${sessionCount} ${l('Sessions')}</a>`;
                        var newSessionButton = `<button class="btn btn-sm btn-primary ms-2 create-session-btn" data-plan-id="${planId}">${l('NewSession')}</button>`;

                        return `${viewSessionsButton} ${newSessionButton}`;
                    },
                    orderable: false
                }, {
                    title: l('Sessions'),
                    rowAction: {
                        items:
                            [

                                {
                                    text: l('New Session'),
                                    action: function (data) {
                                        createSessionModal.open({ planId: data.record.id })
                                            ;
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

    var createModal = new abp.ModalManager(abp.appPath + 'TreatmentPlan/CreateTreatmentPlanModal');
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewTreatmentPlanButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    // ==================== SESSIONS MANAGEMENT ====================

    // View Sessions Button Click
    $('#TreatmentPlansTable').on('click', '.view-sessions', function () {
        currentPlanId = $(this).data('plan-id');
         viewSessionModal.open({ planId: currentPlanId });
        //viewSessionModal.open();
    });



    // Create Session Button Click
    $('#TreatmentPlansTable').on('click', '.create-session-btn', function () {
        var planId = $(this).data('plan-id');
        createSessionModal.open({ planId: planId });
    });

    // Create Session Modal Result
    createSessionModal.onResult(function () {
        abp.notify.success(l('SuccessfullyCreatedSession'));
        viewSessionModal.reopen();
    });

    // Edit Session Modal Result
    editSessionModal.onResult(function () {
        abp.notify.success(l('SuccessfullyUpdatedSession'));
        viewSessionModal.reopen();
    });
}); // ← واحد closing bracket فقط