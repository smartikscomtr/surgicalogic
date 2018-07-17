<template>
  <div>
    <v-card class="grid-card">
      <div class="page-title">
        <h2>
          {{ title }}
        </h2>

        <v-container>
          <v-card-title>
            <v-text-field v-model="search" append-icon="search" label="Search" v-on:keyup.enter="filterGrid" single-line hide-details>
            </v-text-field>

            <v-spacer></v-spacer>

            <v-btn class="orange" slot="activator" @click="addNewItem">
              <v-icon color="white--text">
                add
              </v-icon>
              {{ $t('common.add') }}
            </v-btn>
          </v-card-title>

          <v-data-table :headers="headers" :items="items" :pagination.sync="pagination" :total-items="totalCount" :rows-per-page-items="[10,20,{'text':$t('common.all'),'value':-1}]">
            <template slot="items" slot-scope="props">
              <td v-for="(header, i) in headers" :key="i">
                <template v-if="!header.isAction">
                  {{ props.item[header.value] }}
                </template>

                <template v-else>
                  <v-btn v-if="showDetail" icon class="mx-0" @click="detailItem(props.item)">
                    <v-icon color="#232222">
                      visibility
                    </v-icon>
                  </v-btn>

                  <v-btn v-if="showEdit" icon class="mx-0" @click="editItem(props.item)">
                    <v-icon color="#232222">
                      edit
                    </v-icon>
                  </v-btn>

                  <v-btn v-if="showDelete" icon class="mx-0" @click="deleteItem(props.item)">
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
            loading: true,
            dataRows: [],
            pagination: {}
        };
    },

    watch: {
        pagination: {
            handler() {
               const vm = this;
               vm.executeGridOperations();
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
            vm.$emit('edit', item);
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
                search: vm.search
            });
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
    padding-left: 16px;
    padding-top: 20px;
}
.datatable__actions {
    background-color: #f8f8f8 !important;
    padding-top: 10px;
}
.table__overflow {
    margin-top: 36px;
}
table.table thead tr {
    height: 70px;
}
table.table thead th {
    font-weight: bold;
    font-size: initial;
}
table.table thead td {
    font-weight: 400;
    font-size: 75%;
}
table.table tbody td:last-child {
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
.card {
    background-color: #fff;
    position: relative;
    margin-top: 0;
    margin-bottom: 0;
    padding: 12px 42px;
}
.card__title {
    display: flex;
    align-items: center;
    justify-content: space-between;
    flex-direction: row;
}
.card__title .flex {
    padding: 0 20px;
}
.card__text .flex {
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
.orange .btn__content {
    color: #fff;
}
.headline-wrap .btn--active .btn__content:before,
.headline-wrap .btn:focus .btn__content:before,
.headline-wrap .btn:hover .btn__content:before {
    background-color: transparent;
}
.headline-wrap span.text {
    margin: 0 10px;
}
.headline-wrap .backBtn i {
    color: #000 !important;
}
.headline-wrap .btn__content {
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
.dialog.dialog--active,
.dialog:not(.dialog--fullscreen) {
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
.dialog:not(.dialog--fullscreen) {
    max-height: inherit;
}
.dialog:not(.dialog--fullscreen) .card__text {
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
table.table tbody td:first-child,
table.table tbody td:not(:first-child),
table.table tbody th:first-child,
table.table tbody th:not(:first-child),
table.table thead td:first-child,
table.table thead td:not(:first-child),
table.table thead th:first-child,
table.table thead th:not(:first-child) {
    padding: 0px 15px;
    white-space: nowrap;
}
</style>
