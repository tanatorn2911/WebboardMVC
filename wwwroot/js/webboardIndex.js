$(document).ready(function () {
    $(document).on('click', '.btndeletepost', function () {
        console.log("Button clicked");  // ใช้เพื่อทดสอบว่าปุ่มถูกคลิก
        const button = $(this); // รับปุ่มที่ถูกคลิก
        confirmDelete(button);
    });
});

function confirmDelete(button) {
    const form = button.closest('form'); // หา form ที่ใกล้ที่สุดของปุ่มที่ถูกคลิก
    Swal.fire({
        title: 'ยืนยันการลบ?',
        text: 'คุณแน่ใจหรือไม่ว่าต้องการลบรายการนี้?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'ลบ',
        cancelButtonText: 'ยกเลิก'
    }).then((result) => {
        if (result.isConfirmed) {
            form.submit(); // ถ้า confirmed ให้ส่ง form
        }
    });
}
function validateEmail(inputId, errorMessageId, errorMessage) {
    var email = $('#' + inputId).val().trim();
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    let isEmail = regex.test(email)
    if (!isEmail) {
        $('#' + errorMessageId).text(errorMessage);
    } else if (email.length > 150) {
        $('#' + errorMessageId).text('Max 150 character')
    } else {
        $('#' + errorMessageId).text('');
    }
    return isEmail;
}
