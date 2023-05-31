window.onload = () => {
    setTimeout(() => {
        if (document.querySelectorAll(".chat-msg-box").length == 0) {
            $.ajax({
                url: "./api/welcome",
                beforeSend: () => {
                    $(".typing").show();
                    $("main").append(`
            <div class="chat-msg-box bot">
              <div class="spinner">
                <div class="bounce1"></div>
                <div class="bounce2"></div>
                <div class="bounce3"></div>
              </div>
            </div>
            `);
                },
                success: (data) => {
                    const response = (data.responseText).replace(/\n/gm, "</br>");
                    $(".chat-msg-box.bot:last-child").html(`<p>${response}</p>`);
                },
                error: () => {
                    $(".chat-msg-box.bot:last-child").remove();
                },
                complete: () => {
                    $(".typing").hide();
                },
            });
        }
    }, 3000);

    $.ajax({
        url: "./api/allquestions",
        success: (data) => {
            data.forEach((qus) => {
                $(".questions.container").append(`
        <div class="question">
          <p>${qus}</p>
        </div>
        `);
            });
        },
    });
};

$("#toogle-chat").on("click", () => {
    toogleShowSuggestions();
});

window.onresize = () => {
    if (window.innerHeight < 580) {
        $("header").css("top", "-4em");
    } else {
        $("header").css("top", "0vh");
    }
};

$("#chat-form").submit((e) => {
    e.preventDefault();
    submitForm();
});

const typed = new Typed(".chat-input", {
    strings: [
        "tôi muốn biết kinh nghiệm chăm sóc chó mèo",
        "cho tôi biết về cộng đồng BDC25",
        "video hướng dẫn",
        "tôi có thể giải trí không",
        "bạn có rảnh không",
        "tôi muốn ủng hộ cho nhóm"
    ],
    typeSpeed: 60,
    backSpeed: 30,
    backDelay: 1500,
    showCursor: true,
    cursorChar: "|",
    attr: "placeholder",
    loop: true,
    bindInputFocusEvents: false,
    shuffle: true,
});
