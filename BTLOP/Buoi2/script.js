document.addEventListener('DOMContentLoaded', () => {
    // Elements Cache
    const body = document.body;
    
    // Yêu Cầu 4: Chuyển đổi giao diện (Dark Mode)
    const themeToggle = document.getElementById('themeToggle');
    let isDarkMode = false;
    
    // Load theme từ localStorage nếu muốn (tùy chọn mở rộng cho xịn)
    if(localStorage.getItem('theme') === 'dark') {
        isDarkMode = true;
        body.setAttribute('data-theme', 'dark');
        themeToggle.textContent = '☀️';
    }

    themeToggle.addEventListener('click', () => {
        isDarkMode = !isDarkMode;
        if(isDarkMode) {
            body.setAttribute('data-theme', 'dark');
            themeToggle.textContent = '☀️';
            localStorage.setItem('theme', 'dark');
        } else {
            body.removeAttribute('data-theme');
            themeToggle.textContent = '🌙';
            localStorage.setItem('theme', 'light');
        }
    });

    // Yêu Cầu 1: Chỉnh sửa thông tin cá nhân (Sử dụng Custom Modal thay vì prompt cho thẩm mỹ)
    const editInfoBtn = document.getElementById('editInfoBtn');
    const editModal = document.getElementById('editModal');
    const saveEditBtn = document.getElementById('saveEditBtn');
    const cancelEditBtn = document.getElementById('cancelEditBtn');
    
    const userNameEl = document.getElementById('userName');
    const userStudentIdEl = document.getElementById('userStudentId');
    const userClassEl = document.getElementById('userClass');
    const quoteAuthorEl = document.getElementById('quoteAuthor');
    
    const inputName = document.getElementById('inputName');
    const inputMSSV = document.getElementById('inputMSSV');
    const inputClass = document.getElementById('inputClass');

    // Mở popup để nhập thông tin
    editInfoBtn.addEventListener('click', () => {
        // Gán giá trị hiện tại vào form
        inputName.value = userNameEl.textContent;
        inputMSSV.value = userStudentIdEl.textContent;
        inputClass.value = userClassEl.textContent;
        
        editModal.classList.remove('hidden');
        inputName.focus();
    });

    // Đóng popup
    cancelEditBtn.addEventListener('click', () => {
        editModal.classList.add('hidden');
    });

    // Lưu thông tin vẳ cập nhật ngay lên trang
    saveEditBtn.addEventListener('click', () => {
        const newName = inputName.value.trim();
        const newMSSV = inputMSSV.value.trim();
        const newClass = inputClass.value.trim();
        
        // Cập nhật DOM - Yêu cầu: Nội dung hiển thị cập nhật ngay sau khi nhập
        if(newName) {
            userNameEl.textContent = newName;
            quoteAuthorEl.textContent = newName; 
        }
        if(newMSSV) userStudentIdEl.textContent = newMSSV;
        if(newClass) userClassEl.textContent = newClass;
        
        editModal.classList.add('hidden');
    });

    // Yêu Cầu 2: Thêm sở thích cá nhân
    const hobbyList = document.getElementById('hobbyList');
    const newHobbyInput = document.getElementById('newHobbyInput');
    const addHobbyBtn = document.getElementById('addHobbyBtn');
    const hobbyError = document.getElementById('hobbyError');

    const addNewHobby = () => {
        const newHobby = newHobbyInput.value.trim();
        
        // Yêu cầu: Không chấp nhận trường hợp nhập rỗng.
        if (!newHobby) {
            hobbyError.classList.remove('hidden');
            // Thêm animation báo lỗi (lắc nhẹ input)
            newHobbyInput.style.transform = "translateX(-5px)";
            setTimeout(() => newHobbyInput.style.transform = "translateX(5px)", 100);
            setTimeout(() => newHobbyInput.style.transform = "translateX(0)", 200);
        } else {
            // Check pass
            hobbyError.classList.add('hidden');
            
            // Yêu cầu: Hiển thị ngay trên trang dưới dạng danh sách
            const li = document.createElement('li');
            li.textContent = newHobby;
            
            // Effect hiện ra đẹp mắt
            li.style.opacity = 0;
            li.style.transform = "translateX(-20px)";
            li.style.transition = "all 0.4s ease";
            
            hobbyList.appendChild(li);
            
            newHobbyInput.value = '';
            newHobbyInput.focus();
            
            // Trigger reflow cho animation hoạt động
            setTimeout(() => {
                li.style.opacity = 1;
                li.style.transform = "translateX(0)";
            }, 10);
        }
    };

    addHobbyBtn.addEventListener('click', addNewHobby);
    
    // Bắt sự kiện phím Enter
    newHobbyInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            addNewHobby();
        }
    });

    // Yêu Cầu 3: Ẩn/Hiện đoạn mã nguồn (Source Code Toggle)
    const toggleCodeBtn = document.getElementById('toggleCodeBtn');
    const codeBlock = document.getElementById('codeBlock');
    const codeStatusText = document.getElementById('codeStatusText');
    let isCodeVisible = true;
    
    toggleCodeBtn.addEventListener('click', () => {
        isCodeVisible = !isCodeVisible;
        
        // Yêu cầu: Trạng thái hiển thị thay đổi mỗi lần nhấn
        if (isCodeVisible) {
            codeBlock.classList.remove('hidden');
            toggleCodeBtn.textContent = 'Ẩn mã nguồn';
            toggleCodeBtn.classList.remove('primary-btn');
            toggleCodeBtn.classList.add('outline-btn');
            if (codeStatusText) {
                codeStatusText.textContent = 'Đang hiện';
                codeStatusText.className = 'text-success';
                codeStatusText.style.fontWeight = '600';
            }
        } else {
            codeBlock.classList.add('hidden');
            toggleCodeBtn.textContent = 'Hiện mã nguồn';
            toggleCodeBtn.classList.remove('outline-btn');
            toggleCodeBtn.classList.add('primary-btn');
            if (codeStatusText) {
                codeStatusText.textContent = 'Đang ẩn';
                codeStatusText.className = 'text-danger';
                codeStatusText.style.fontWeight = '600';
            }
        }
    });
});
