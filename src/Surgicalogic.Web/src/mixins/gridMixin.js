export const gridMixin = {
  methods: {
    detail(payload) {
      const vm = this;

      vm.detailDialog = true;
      vm.detailAction = payload;
    },

    edit(payload, itemIndex){
      const vm = this;
      vm.editDialog = true;
      vm.editedIndex = itemIndex;
      vm.editAction = Object.assign({}, payload);
    },

    editAvailability(payload, itemIndex){
      const vm = this;

      vm.editAvailableDialog = true;
      vm.editedAvailableIndex = itemIndex;
      vm.editAvailableAction = Object.assign({}, payload);
    },

    cancel() {
      const vm = this;

      vm.calendarDialog = false;
      vm.detailDialog = false;
      vm.editDialog = false;
      vm.editAvailableDialog = false;
      vm.deleteDialog = false;
      vm.deleteAvailabilityDialog = false;
      vm.resetPasswordDialog = false;
      vm.planOperationDialog = false;

      setTimeout(() =>{
        vm.editAction = Object.assign({}, {})
      }, 300);
    },

    addNewItem(){
      const vm = this;

      vm.editedIndex = -1;
      vm.editDialog = true;

      vm.editAction = {};
    },

    addNewCalendarItem(){
      const vm = this;

      vm.editAvailableIndex = -1;
      vm.editAvailableDialog = true;

      vm.editAvailableAction = {};
    },

    deleteItem(payload) {
      const vm = this;

      vm.deleteValue = payload;
      vm.deleteDialog = true;
    },

    deleteCalendarItem(payload) {
      const vm = this;

      vm.deleteAvailabilityValue = payload;
      vm.deleteAvailabilityDialog = true;
    },

    resetPassword(payload) {
      const vm = this;

      vm.resetPasswordValue = payload;
      vm.resetPasswordDialog = true;
    }
  }
}
