/*document.getElementById("sendButton").onclick = function () {
    // Get the Antiforgery token value from the form
    const antiforgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

    // Collect the form data
    var formData = {
        Id: document.getElementById("Id").value,
        AssignedTo: document.getElementById("Assignto").value,
        AssignedBy: document.getElementById("Assignby").value,
        Message: document.getElementById("Message").value
    };

    // Perform an AJAX POST request to the controller action when the button is clicked
    fetch('/AdaptiveCard/SendAdaptiveCard', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': antiforgeryToken // Include the Antiforgery token in the headers
        },
        body: JSON.stringify(formData) // Send the form data in the request body
    })
        .then(response => response.text())
        .then(data => alert(data))
        .catch(error => alert('An error occurred while sending the adaptive card.'));
};



*/