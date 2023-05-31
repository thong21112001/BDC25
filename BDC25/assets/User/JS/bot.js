document.addEventListener("DOMContentLoaded", function () {
    var chatbotHeader = document.querySelector(".chatbot__header");
    var chatbot = document.querySelector(".chatbot");
    var inputAnswer = document.querySelector(".Input_answer");
    var inputQuestion = document.querySelector(".Input_question");
    var iconToggle = document.querySelector("#toogle-chat");

    var chatLog = document.querySelector('.chat-log');
    var userInput = document.querySelector('.user-input input');
    var sendButton = document.querySelector('.user-input button');

    chatbotHeader.addEventListener("click", function () {
        if (inputAnswer.classList.contains("hidden")) {
            inputAnswer.classList.remove("hidden");
            inputQuestion.classList.remove("hidden");
            chatbot.style.height = "auto";
            chatbot.style.width = "auto";
        } else {
            inputAnswer.classList.add("hidden");
            inputQuestion.classList.add("hidden");
            chatbot.style.height = "500px";
            chatbot.style.width = "20rem";
        }
        if (iconToggle.innerHTML === '<i class="fa-solid fa-xmark"></i>') {
            iconToggle.innerHTML = '<i class="fa-regular fa-comment"></i>';
        } else {
            iconToggle.innerHTML = '<i class="fa-solid fa-xmark"></i>';
        }
    });
});