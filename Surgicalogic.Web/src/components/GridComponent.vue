<template>
  <div>
    <v-card class="grid-card">
      <div class="page-title">
        <h2>
          {{ title }}
        </h2>

        <v-container>
          <v-card-title class="search-wrap">
            <v-text-field v-model="search"
                          append-icon="search"
                          label="Search"
                          v-on:keyup.enter="filterGrid"
                          single-line hide-details>
            </v-text-field>

            <v-spacer></v-spacer>

            <v-btn class="orange"
                   slot="activator"
                   @click="addNewItem">
              <v-icon color="white--text">
                add
              </v-icon>
              {{ $t('common.add') }}
            </v-btn>
          </v-card-title>

          <v-data-table :headers="headers"
                        :items="items"
                        :loading="loading"
                        :pagination.sync="pagination"
                        :total-items="totalCount"
                        :rows-per-page-items="[10, 20, { 'text': $t('common.all'), 'value': -1 }]">
            <v-progress-linear slot="progress"
                               color="teal"
                               indeterminate>
            </v-progress-linear>

            <template slot="items" slot-scope="props">
              <td v-for="(header, i) in headers"
                  :key="i">
                <template v-if="!header.isAction && header.isCheck">
                  <div v-if="props.item[header.value]">
                    <v-icon>check</v-icon>
                  </div>

                  <div v-else>
                    <v-icon>close</v-icon>
                  </div>
                </template>

                <template v-else-if="!header.isAction">
                  {{ props.item[header.value] }}
                </template>

                <template v-else>
                  <v-btn v-if="showDetail"
                         icon
                         class="mx-0"
                         @click="detailItem(props.item)">
                    <v-icon color="#232222">
                      visibility
                    </v-icon>
                  </v-btn>

                  <v-btn v-if="showEdit"
                         icon
                         class="mx-0"
                         @click="editItem(props.item)">
                    <v-icon color="#232222">
                      edit
                    </v-icon>
                  </v-btn>

                  <v-btn v-if="showDelete"
                         icon
                         class="mx-0"
                         @click="deleteItem(props.item)">
                    <v-icon color="#232222">
                      delete
                    </v-icon>
                  </v-btn>
                </template>
              </td>
            </template>
          </v-data-table>
        </v-container>
      </div>
    </v-card>
  </div>
</template>

<script>

export default {
  props: {
    items: {
      type: Array,
      required: false,
      default() {
        return [];
      }
    },

    loading: {
      type: Boolean,
      required: false
    },

    totalCount: {
      type: Number,
      required: false
    },

    headers: {
      type: Array,
      required: false
    },

    title: {
      type: String,
      required: false
    },

    showDetail: {
      type: Boolean,
      required: false
    },

    showEdit: {
      type: Boolean,
      required: false
    },

    showDelete: {
      type: Boolean,
      required: false
    },

    methodName: {
      type: Function,
      required: true
    }
  },

  data() {
    return {
      search: '',
      dataRows: [],
      pagination: {},
      pagingExecuted: false
    };
  },

  watch: {
    pagination: {
      handler(newValue, oldValue) {
        const vm = this;

        if (!oldValue.rowsPerPage || (oldValue.rowsPerPage && vm.pagingExecuted)) {
          vm.executeGridOperations();
        } else {
          vm.pagingExecuted = true;
        }
      }
    }
  },

  methods: {
    addNewItem() {
      const vm = this;

      //When the add new button is clicked, the event is sent to the grid component
      vm.$emit('newaction');
    },

    filterGrid() {
      const vm = this;

      vm.executeGridOperations();
    },

    detailItem(item) {
      const vm = this;

      //When the detail button is clicked, the event is sent to the grid component
      vm.$emit('detail', item);
    },

    editItem(item) {
      const vm = this;
      //When the edit button is clicked, the event is sent to the grid component
      vm.$emit('edit', item, vm.items.indexOf(item));
    },

    deleteItem(item) {
      const vm = this;

      //When the delete button is clicked, the event is sent to the grid component
      vm.$emit('deleteitem', item);
    },

    executeGridOperations() {
      const vm = this;

      const { sortBy, descending, page, rowsPerPage } = vm.pagination;

      vm.$store.dispatch(vm.methodName(), {
        currentPage: page,
        pageSize: rowsPerPage,
        search: vm.search,
        sortBy: vm.getSortByField(sortBy),
        descending: descending
      });
    },

    getSortByField(sortBy) {
      const vm = this;

      vm.headers.forEach(element => {
        if (element.value == sortBy && element.sortBy) {
          sortBy = element.sortBy;
        }
      });

      return sortBy;
    }
  }
};

</script>

<style>
.grid-card.card {
  box-shadow: inherit;
  height: 100vh !important;
}
.grid-card .page-title h2 {
  padding-left: 24px;
  padding-top: 20px;
}
.datatable__actions {
  background-color: #f8f8f8 !important;
  padding-top: 10px;
}
.table__overflow {
  margin-top: 36px;
}
table.v-table thead tr {
  height: 70px;
}
table.v-table thead th {
  font-weight: bold;
  font-size: initial;
}
table.v-table thead td {
  font-weight: 400;
  font-size: 75%;
}
table.v-table tbody td:last-child {
  text-align: right;
}
tr:nth-child(even) {
  background-color: #f2f2f2;
}
.headline-wrap {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
}
.v-card {
  background-color: #fff;
  position: relative;
  margin-top: 0;
  margin-bottom: 0;
  box-shadow: none;
}
.grid-card.v-card {
  padding: 0 20px;
}
.v-card__title {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-direction: row;
}
.v-card__title.search-wrap {
  padding: 16px 0;
}
.v-card__title .flex {
  padding: 0 16px;
}
.v-card__text .flex {
  padding: 0 20px !important;
}
.text {
  flex: 1;
  font-size: 18px;
  color: #000;
  margin: 0;
}
.orange {
  padding: 0;
  margin: 0;
  min-width: 140px;
  background-color: #ff7107 !important;
  height: 40px;
  font-size: 15px;
}
.orange .v-btn__content {
  color: #fff;
}
.close-wrap{
  color: #ff7107 !important;
}
.headline-wrap .btn--active .btn__content:before,
.headline-wrap .btn:focus .btn__content:before,
.headline-wrap .btn:hover .btn__content:before {
  background-color: transparent;
}
.headline-wrap span.text {
  /* margin: 0 10px; */
}
.headline-wrap .backBtn i {
  color: #000 !important;
}
.headline-wrap .v-btn__content {
  will-change: box-shadow;
  -webkit-box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2),
      0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12);
  box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2),
      0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12);
}
.label {
  font-size: 12px;
  line-height: 1.33;
  text-align: left;
  color: #464646;
}
.value {
  font-size: 16px;
  line-height: 1.5;
  text-align: left;
  color: #000;
}
.v-dialog.v-dialog--active,
.v-dialog:not(.v-dialog--fullscreen) {
  max-width: 600px;
}
a {
  color: #009688;
}
.primary {
  background-color: #009688 !important;
  border-color: #009688 !important;
}
.primary--text {
  color: #009688 !important;
}
.primary--text input,
.primary--text textarea {
  caret-color: #009688 !important;
}
.primary--after::after {
  background: #009688 !important;
}
.input-group-checkbox {
  display: flex;
  align-items: center;
}
.readonly-wrap {
  flex-direction: column;
}
.input-group.readonly-wrap {
  margin-bottom: 10px;
}
.v-dialog:not(.dialog--fullscreen) {
  max-height: inherit;
}
.v-dialog:not(.v-dialog--fullscreen) .v-card__text {
  max-height: 50vh;
  overflow-y: auto;
}
.input-group__selections > div {
  display: inline-block !important;
  width: 100%;
  white-space: nowrap;
  overflow: hidden !important;
  text-overflow: ellipsis;
}
table.v-table tbody td:first-child,
table.v-table tbody td:not(:first-child),
table.v-table tbody th:first-child,
table.v-table tbody th:not(:first-child),
table.v-table thead td:first-child,
table.v-table thead td:not(:first-child),
table.v-table thead th:first-child,
table.v-table thead th:not(:first-child) {
  padding: 0px 15px;
  white-space: nowrap;
}
.navigation i,
.navigation .v-list__tile__title {
  color: #fff !important;
}
.application .theme--light.v-list .v-list__group__header:hover,
.application .theme--light.v-list .v-list__tile--highlighted,
.application .theme--light.v-list .v-list__tile--link:hover,
.theme--light .v-list .v-list__group__header:hover,
.theme--light .v-list .v-list__tile--highlighted,
.theme--light .v-list .v-list__tile--link:hover {
  background: rgba(0, 0, 0, 0.1);
}
.v-navigation-drawer .v-list {
  background: #333;
}
.v-list__group.v-list__group--active {
  background: #262626;
}
.v-card__title .v-btn__content {
  color: #fff;
}

.v-card__title .v-input__slot {
  width: 80%;
}
.v-input__slot {
  margin-bottom: 0;
}
.v-toolbar__content .v-input {
  position: relative;
}
.v-toolbar__content .v-input .v-input__prepend-outer {
  position: absolute;
  right: 10px;
  top: 0;
}
.v-toolbar__content .v-toolbar__title button {
  margin-left: -11px;
}
.v-toolbar__content .v-toolbar__title {
  overflow: visible;
}
</style>
