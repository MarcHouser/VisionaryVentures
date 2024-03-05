// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
if ($('.chat-box').length) {
    $('.chat-box').scrollTop($('.chat-box')[0].scrollHeight);
}

function populateFields() {
        // Sample data for populating the form
    document.getElementById('firstNameInput').value = "John";
    document.getElementById('lastNameInput').value = "Doe";
    document.getElementById('PhoneNumberInput').value = "123-456-7890";
    document.getElementById('StreetAddressID').value = "1234 Main St";
    document.getElementById('CityInput').value = "Anytown";
    document.getElementById('StateInput').value = "State";
    document.getElementById('PostalCodeInput').value = "12345";
    document.getElementById('CountryInput').value = "Country";
    document.getElementById('UserTypeInput').value = "Employee";
    document.getElementById('UserEmailInput').value = "johndoe@example.com";
    document.getElementById('UsernameInput').value = "johndoe";
    document.getElementById('PasswordInput').value = "password";
}

function clearFields() {
        // Clearing all the form fields
    document.getElementById('firstNameInput').value = "";
    document.getElementById('lastNameInput').value = "";
    document.getElementById('PhoneNumberInput').value = "";
    document.getElementById('StreetAddressID').value = "";
    document.getElementById('CityInput').value = "";
    document.getElementById('StateInput').value = "";
    document.getElementById('PostalCodeInput').value = "";
    document.getElementById('CountryInput').value = "";
    document.getElementById('UserTypeInput').value = "";
    document.getElementById('UserEmailInput').value = "";
    document.getElementById('UsernameInput').value = "";
    document.getElementById('PasswordInput').value = "";
}

function populateKnowledgeFields() {
    // Populate the form fields with example data
    document.getElementById('TitleInput').value = "Sample Title";
    document.getElementById('CategoryInput').value = "Sample Category";
    document.getElementById('InformationInput').value = "This is sample information for the knowledge item.";
}

function clearKnowledgeFields() {
    // Clear all the form fields
    document.getElementById('TitleInput').value = "";
    document.getElementById('CategoryInput').value = "";
    document.getElementById('InformationInput').value = "";
}

function populateSWOTFields() {
    document.getElementById('StrengthInput').value = "Sample Strength";
    document.getElementById('WeaknessInput').value = "Sample Weakness";
    document.getElementById('OpportunityInput').value = "Sample Opportunity";
    document.getElementById('ThreatInput').value = "Sample Threat";
}

function clearSWOTFields() {
    document.getElementById('StrengthInput').value = "";
    document.getElementById('WeaknessInput').value = "";
    document.getElementById('OpportunityInput').value = "";
    document.getElementById('ThreatInput').value = "";
}

function populatePlanFields() {
    document.getElementById('PlanNameInput').value = "Sample Plan Name";
    document.getElementById('PlanDescriptionInput').value = "This is a sample plan description.";
}

function clearPlanFields() {
    document.getElementById('PlanNameInput').value = "";
    document.getElementById('PlanDescriptionInput').value = "";
}

function toggleFormVisibility() {
    var knowledgeItemBtn = document.getElementById('showKnowledgeItemFormBtn');
    var planBtn = document.getElementById('showPlanFormBtn');
    var datasetBtn = document.getElementById('showDatasetFormBtn');

    knowledgeItemBtn.addEventListener('click', function () {
        var form = document.getElementById('knowledgeItemForm');
        form.style.display = form.style.display === 'none' ? 'block' : 'none';
    });

    planBtn.addEventListener('click', function () {
        var form = document.getElementById('planForm');
        form.style.display = form.style.display === 'none' ? 'block' : 'none';
    });

    datasetBtn.addEventListener('click', function () {
        var form = document.getElementById('datasetForm');
        form.style.display = form.style.display === 'none' ? 'block' : 'none';
    });
}

document.addEventListener('DOMContentLoaded', toggleFormVisibility);

function filterTableRows() {
    var searchValue = document.getElementById('data-search').value.toLowerCase();
    var tableRows = document.querySelectorAll('.data-set tbody tr');

    tableRows.forEach(function (row) {
        var rowText = row.textContent.toLowerCase();
        row.style.display = rowText.includes(searchValue) ? '' : 'none';
    });
}

function filterDatasetButtons() {
    var searchValue = document.getElementById('sidebar-search').value.toLowerCase();
    var datasetButtons = document.querySelectorAll('.dataset-btn');

    datasetButtons.forEach(function (button) {
        var buttonText = button.textContent.toLowerCase();
        button.style.display = buttonText.includes(searchValue) ? '' : 'none';
    });
}

function filterCollaborations() {
    var searchValue = document.getElementById('collaboration-search').value.toLowerCase();
    var collaborationButtons = document.querySelectorAll('.nav-pills .btn'); // Adjust the selector as needed

    collaborationButtons.forEach(function (button) {
        var buttonText = button.textContent.toLowerCase();
        button.style.display = buttonText.includes(searchValue) ? '' : 'none';
    });
}
document.getElementById('search-form').addEventListener('submit', function (event) {
    event.preventDefault(); // Prevent the default form submission
    filterCollaborations();
});

function populateUserFields() {
    document.getElementById('FirstName').value = 'John';
    document.getElementById('LastName').value = 'Doe';
    document.getElementById('EmailAddress').value = 'john.doe@example.com';
    document.getElementById('PhoneNumber').value = '123-456-7890';
    document.getElementById('StreetAddress').value = '1234 Main St';
    document.getElementById('City').value = 'Anytown';
    document.getElementById('State').value = 'Anystate';
    document.getElementById('PostalCode').value = '12345';
    document.getElementById('Country').value = 'CountryName';
}

function clearUserFields() {
    document.getElementById('FirstName').value = '';
    document.getElementById('LastName').value = '';
    document.getElementById('EmailAddress').value = '';
    document.getElementById('PhoneNumber').value = '';
    document.getElementById('StreetAddress').value = '';
    document.getElementById('City').value = '';
    document.getElementById('State').value = '';
    document.getElementById('PostalCode').value = '';
    document.getElementById('Country').value = '';
}
function filterPlans() {
    var searchValue = document.getElementById('plan-search').value.toLowerCase();
    var planButtons = document.querySelectorAll('.nav-pills .btn');

    planButtons.forEach(function (button) {
        var buttonText = button.textContent.toLowerCase();
        if (buttonText.includes(searchValue)) {
            button.style.display = '';
        } else {
            button.style.display = 'none';
        }
    });
}

document.getElementById('search-form').addEventListener('submit', function (event) {
    event.preventDefault(); // Prevent the default form submission
    filterPlans();
});
function filterKnowledgeItems() {
    var searchValue = document.getElementById('knowledge-search').value.toLowerCase();
    var knowledgeButtons = document.querySelectorAll('.nav-pills .btn');

    knowledgeButtons.forEach(function (button) {
        var buttonText = button.textContent.toLowerCase();
        if (buttonText.includes(searchValue)) {
            button.style.display = '';
        } else {
            button.style.display = 'none';
        }
    });
}
document.getElementById('knowledge-search').addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        filterKnowledgeItems();
    }
});