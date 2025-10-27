$(function () {
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
                }
               

            ]
        })
    );
});
