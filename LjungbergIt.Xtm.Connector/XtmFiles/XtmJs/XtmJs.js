function test(element)
{
  alert(element.children[0]);
}

//function showElement(element, className) {
//  if (element.checked) {
//    document.getElementsByClassName(className)[0].style.visibility = 'visible';
//  }
//  else {
//    document.getElementsByClassName(className)[0].style.visibility = 'hidden';
//  }
//}

function showElementById(element, elementToShowId) {
  if (element.checked) {
    document.getElementById(elementToShowId).style.display = 'block';
  }
  else {
    document.getElementById(elementToShowId).style.display = 'none';
  }
}

//function showElementByClass(classOfCheckboxElement, classOfElementToShowOrHide) {
//  if (classOfCheckboxElement.checked) {
//    document.getElementsByClassName(classOfElementToShowOrHide)[0].style.display = '';
//  }
//  else {
//    document.getElementsByClassName(classOfElementToShowOrHide)[0].style.visibility = 'none';
//  }
//}

function showElementGlobal(elementId, showElementId) {
  if (document.getElementById(elementId).checked) {
    document.getElementById(showElementId).style.visibility = 'visible';
  }
  else {
    document.getElementById(showElementId).style.visibility = 'hidden';
  }
}