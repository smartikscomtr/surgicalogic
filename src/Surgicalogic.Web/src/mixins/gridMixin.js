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

    cancel() {
      const vm = this;

      vm.calendarDialog = false;
      vm.detailDialog = false;
      vm.editDialog = false;
      vm.deleteDialog = false;
      vm.resetPasswordDialog = false;

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

    deleteItem(payload) {
      const vm = this;

      vm.deleteValue = payload;
      vm.deleteDialog = true;
    },

    resetPassword(payload) {
      const vm = this;

      vm.resetPasswordValue = payload;
      vm.resetPasswordDialog = true;
    }
  }
}
